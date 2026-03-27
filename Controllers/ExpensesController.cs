using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId, DateTime? startDate, DateTime? endDate)
        {
            var expenses = _context.Expenses.Include(e => e.ExpenseCategory).AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                expenses = expenses.Where(e => e.ExpenseCategoryId == categoryId.Value);
            }

            if (startDate.HasValue)
            {
                expenses = expenses.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                expenses = expenses.Where(e => e.Date <= endDate.Value);
            }

            ViewBag.Categories = await _context.ExpenseCategories.Where(c => c.IsActive).ToListAsync();
            ViewBag.TotalAmount = await expenses.SumAsync(e => e.Amount);
            ViewBag.SelectedCategory = categoryId;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(await expenses.OrderByDescending(e => e.Date).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.ExpenseCategories.Where(c => c.IsActive).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                expense.CreatedBy = User.Identity?.Name ?? "Unknown";
                expense.CreatedDate = DateTime.UtcNow;
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.ExpenseCategories.Where(c => c.IsActive).ToListAsync();
            return View(expense);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _context.ExpenseCategories.Where(c => c.IsActive).ToListAsync();
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    expense.ModifiedDate = DateTime.UtcNow;
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.ExpenseCategories.Where(c => c.IsActive).ToListAsync();
            return View(expense);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.ExpenseCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reports()
        {
            var expenses = await _context.Expenses
                .Include(e => e.ExpenseCategory)
                .ToListAsync();

            var categoryTotals = expenses
                .GroupBy(e => e.ExpenseCategory?.Name ?? "Uncategorized")
                .Select(g => new CategoryTotal
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Amount),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            var monthlyTotals = expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new MonthlyTotal
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();

            var model = new ReportsViewModel
            {
                GrandTotal = expenses.Sum(e => e.Amount),
                CategoryTotals = categoryTotals,
                MonthlyTotals = monthlyTotals
            };

            return View(model);
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
