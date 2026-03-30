using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class BabyItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BabyItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category = "All", string sortBy = "Priority")
        {
            var items = _context.BabyItems.AsQueryable();

            if (category != "All")
            {
                items = items.Where(i => i.Category == category);
            }

            items = sortBy switch
            {
                "Name" => items.OrderBy(i => i.Name),
                "Priority" => items.OrderBy(i => i.Priority),
                "Status" => items.OrderBy(i => i.IsPurchased),
                _ => items.OrderBy(i => i.Priority)
            };

            ViewBag.Categories = await _context.BabyItems
                .Select(i => i.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return View(await items.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BabyItem item)
        {
            if (ModelState.IsValid)
            {
                item.CreatedBy = User.Identity?.Name ?? "Unknown";
                item.CreatedDate = DateTime.UtcNow;
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAjax([FromForm] BabyItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.CreatedBy = User.Identity?.Name ?? "Unknown";
                    item.CreatedDate = DateTime.UtcNow;
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartial([FromForm] BabyItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.CreatedBy = User.Identity?.Name ?? "Unknown";
                    item.CreatedDate = DateTime.UtcNow;
                    _context.Add(item);
                    await _context.SaveChangesAsync();

                    return PartialView("_BabyItemCard", item);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.BabyItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BabyItem item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    item.ModifiedDate = DateTime.UtcNow;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BabyItemExists(item.Id))
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
            return View(item);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.BabyItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.BabyItems.FindAsync(id);
            if (item != null)
            {
                _context.BabyItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPurchased(int id)
        {
            var item = await _context.BabyItems.FindAsync(id);
            if (item != null)
            {
                item.IsPurchased = !item.IsPurchased;
                item.PurchasedDate = item.IsPurchased ? DateTime.UtcNow : null;
                item.PurchasedBy = item.IsPurchased ? User.Identity?.Name : null;
                item.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BabyItemExists(int id)
        {
            return _context.BabyItems.Any(e => e.Id == id);
        }
    }
}
