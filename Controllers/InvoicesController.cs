using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;
using Ervilhinha.Services;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvoiceOcrService _ocrService;
        private readonly IWebHostEnvironment _environment;

        public InvoicesController(ApplicationDbContext context, IInvoiceOcrService ocrService, IWebHostEnvironment environment)
        {
            _context = context;
            _ocrService = ocrService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Expense)
                .OrderByDescending(i => i.UploadDate)
                .ToListAsync();

            return View(invoices);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View();
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Only image files (JPG, PNG) and PDF are allowed.");
                return View();
            }

            try
            {
                // Create uploads directory if it doesn't exist
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "invoices");
                Directory.CreateDirectory(uploadsPath);

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Process with OCR
                InvoiceOcrResult ocrResult;
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    ocrResult = await _ocrService.ProcessInvoiceAsync(stream);
                }

                // Create invoice record
                var invoice = new Invoice
                {
                    FileName = file.FileName,
                    FilePath = $"/uploads/invoices/{fileName}",
                    UploadDate = DateTime.UtcNow,
                    UploadedBy = User.Identity?.Name ?? "Unknown",
                    IsProcessed = ocrResult.Success,
                    HasErrors = !ocrResult.Success || string.IsNullOrEmpty(ocrResult.VendorName) || !ocrResult.TotalAmount.HasValue,
                    ErrorMessage = ocrResult.ErrorMessage,
                    TotalAmount = ocrResult.TotalAmount,
                    InvoiceDate = ocrResult.InvoiceDate,
                    VendorName = ocrResult.VendorName,
                    RawOcrText = ocrResult.RawText
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                // Auto-create expense if OCR was successful
                if (ocrResult.Success && ocrResult.TotalAmount.HasValue)
                {
                    var expense = new Expense
                    {
                        Description = $"Invoice from {ocrResult.VendorName ?? "Unknown"}",
                        Amount = ocrResult.TotalAmount.Value,
                        Date = ocrResult.InvoiceDate ?? DateTime.Today,
                        ExpenseCategoryId = 0, // Will need to be set in review
                        Notes = $"Auto-generated from invoice. Needs category assignment.",
                        CreatedBy = User.Identity?.Name ?? "Unknown",
                        CreatedDate = DateTime.UtcNow
                    };

                    _context.Expenses.Add(expense);
                    await _context.SaveChangesAsync();

                    invoice.ExpenseId = expense.Id;
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Invoice uploaded and processed successfully!";
                return RedirectToAction(nameof(Review), new { id = invoice.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error processing invoice: {ex.Message}");
                return View();
            }
        }

        public async Task<IActionResult> Review(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Expense)
                .ThenInclude(e => e!.ExpenseCategory)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.ExpenseCategories.Where(c => c.IsActive).ToListAsync();
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, Invoice model, int? expenseCategoryId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Expense)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            // Update invoice details
            invoice.VendorName = model.VendorName;
            invoice.TotalAmount = model.TotalAmount;
            invoice.InvoiceDate = model.InvoiceDate;
            invoice.IsReviewed = true;
            invoice.ReviewedBy = User.Identity?.Name;
            invoice.ReviewedDate = DateTime.UtcNow;
            invoice.HasErrors = false;

            // Update or create expense
            if (invoice.Expense != null)
            {
                invoice.Expense.Description = $"Invoice from {model.VendorName}";
                invoice.Expense.Amount = model.TotalAmount ?? 0;
                invoice.Expense.Date = model.InvoiceDate ?? DateTime.Today;
                
                if (expenseCategoryId.HasValue && expenseCategoryId.Value > 0)
                {
                    invoice.Expense.ExpenseCategoryId = expenseCategoryId.Value;
                    invoice.Expense.Notes = $"Reviewed and categorized from invoice.";
                }
            }
            else if (model.TotalAmount.HasValue && expenseCategoryId.HasValue && expenseCategoryId.Value > 0)
            {
                var expense = new Expense
                {
                    Description = $"Invoice from {model.VendorName}",
                    Amount = model.TotalAmount.Value,
                    Date = model.InvoiceDate ?? DateTime.Today,
                    ExpenseCategoryId = expenseCategoryId.Value,
                    Notes = $"Added from reviewed invoice.",
                    CreatedBy = User.Identity?.Name ?? "Unknown",
                    CreatedDate = DateTime.UtcNow
                };

                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                invoice.ExpenseId = expense.Id;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Invoice reviewed and expense updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Expense)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Expense)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice != null)
            {
                // Delete associated expense if exists
                if (invoice.Expense != null)
                {
                    _context.Expenses.Remove(invoice.Expense);
                }

                // Delete file
                if (!string.IsNullOrEmpty(invoice.FilePath))
                {
                    var fullPath = Path.Combine(_environment.WebRootPath, invoice.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
