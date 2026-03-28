using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;
using System.Security.Claims;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class BabyShoppingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BabyShoppingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BabyShopping
        public async Task<IActionResult> Index(ShoppingCategory? category, ShoppingPriority? priority, bool? purchased)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = _context.BabyShoppingItems.Where(s => s.UserId == userId).AsQueryable();

            if (category.HasValue)
            {
                items = items.Where(s => s.Category == category.Value);
            }

            if (priority.HasValue)
            {
                items = items.Where(s => s.Priority == priority.Value);
            }

            if (purchased.HasValue)
            {
                items = items.Where(s => s.IsPurchased == purchased.Value);
            }

            var itemList = await items.OrderBy(s => s.Priority).ThenBy(s => s.Category).ToListAsync();

            ViewBag.TotalEstimated = itemList.Sum(i => i.EstimatedCost * i.Quantity);
            ViewBag.TotalPurchased = itemList.Where(i => i.IsPurchased).Sum(i => (i.ActualCost ?? i.EstimatedCost) * i.Quantity);
            ViewBag.TotalPending = itemList.Where(i => !i.IsPurchased).Sum(i => i.EstimatedCost * i.Quantity);
            ViewBag.PurchasedCount = itemList.Count(i => i.IsPurchased);
            ViewBag.TotalCount = itemList.Count;

            return View(itemList);
        }

        // GET: BabyShopping/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BabyShopping/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BabyShoppingItem item)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                item.UserId = userId!;
                item.CreatedDate = DateTime.UtcNow;

                _context.Add(item);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Item adicionado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        // GET: BabyShopping/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.BabyShoppingItems
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (item == null) return NotFound();

            return View(item);
        }

        // POST: BabyShopping/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BabyShoppingItem item)
        {
            if (id != item.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.BabyShoppingItems
                        .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

                    if (existing == null) return NotFound();

                    existing.Name = item.Name;
                    existing.Category = item.Category;
                    existing.Priority = item.Priority;
                    existing.EstimatedCost = item.EstimatedCost;
                    existing.RecommendedTiming = item.RecommendedTiming;
                    existing.Quantity = item.Quantity;
                    existing.IsPurchased = item.IsPurchased;
                    existing.PurchaseDate = item.PurchaseDate;
                    existing.ActualCost = item.ActualCost;
                    existing.IsGift = item.IsGift;
                    existing.Notes = item.Notes;
                    existing.StoreLink = item.StoreLink;
                    existing.ModifiedDate = DateTime.UtcNow;

                    _context.Update(existing);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Item atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            return View(item);
        }

        // POST: BabyShopping/MarkPurchased/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPurchased(int id, decimal? actualCost)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.BabyShoppingItems
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (item == null) return NotFound();

            item.IsPurchased = true;
            item.PurchaseDate = DateTime.Today;
            item.ActualCost = actualCost ?? item.EstimatedCost;
            item.ModifiedDate = DateTime.UtcNow;

            _context.Update(item);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{item.Name} marcado como comprado!";
            return RedirectToAction(nameof(Index));
        }

        // GET: BabyShopping/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.BabyShoppingItems
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (item == null) return NotFound();

            return View(item);
        }

        // POST: BabyShopping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.BabyShoppingItems
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (item != null)
            {
                _context.BabyShoppingItems.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Item removido com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: BabyShopping/SuggestItems
        public IActionResult SuggestItems()
        {
            return View();
        }

        // POST: BabyShopping/AddSuggestedItems
        [HttpPost]
        public async Task<IActionResult> AddSuggestedItems(string[] selectedItems, LifestyleLevel lifestyle)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Items essenciais pré-definidos
            var suggestedItems = GetSuggestedItems(lifestyle);

            foreach (var itemKey in selectedItems)
            {
                if (suggestedItems.ContainsKey(itemKey))
                {
                    var suggestion = suggestedItems[itemKey];
                    _context.BabyShoppingItems.Add(new BabyShoppingItem
                    {
                        UserId = userId!,
                        Name = suggestion.Name,
                        Category = suggestion.Category,
                        Priority = suggestion.Priority,
                        EstimatedCost = suggestion.EstimatedCost,
                        RecommendedTiming = suggestion.RecommendedTiming,
                        Quantity = suggestion.Quantity,
                        Notes = suggestion.Notes,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"{selectedItems.Length} items adicionados com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private Dictionary<string, BabyShoppingItem> GetSuggestedItems(LifestyleLevel lifestyle)
        {
            var costMultiplier = lifestyle switch
            {
                LifestyleLevel.Economico => 0.6m,
                LifestyleLevel.Moderado => 1.0m,
                LifestyleLevel.Confortavel => 1.5m,
                LifestyleLevel.Premium => 2.5m,
                _ => 1.0m
            };

            return new Dictionary<string, BabyShoppingItem>
            {
                { "carrinho", new BabyShoppingItem { Name = "Carrinho de Bebé", Category = ShoppingCategory.Passeio, Priority = ShoppingPriority.Essencial, EstimatedCost = 250m * costMultiplier, Quantity = 1, RecommendedTiming = PurchaseTiming.Mes7 } },
                { "cadeira_auto", new BabyShoppingItem { Name = "Cadeira Auto Grupo 0+", Category = ShoppingCategory.Passeio, Priority = ShoppingPriority.Essencial, EstimatedCost = 150m * costMultiplier, Quantity = 1, RecommendedTiming = PurchaseTiming.AntesNascimento } },
                { "berco", new BabyShoppingItem { Name = "Berço", Category = ShoppingCategory.QuartoBebe, Priority = ShoppingPriority.Essencial, EstimatedCost = 180m * costMultiplier, Quantity = 1, RecommendedTiming = PurchaseTiming.Mes8 } },
                { "bodies_0_3m", new BabyShoppingItem { Name = "Bodies 0-3M (pack 5)", Category = ShoppingCategory.Roupa0a3, Priority = ShoppingPriority.Essencial, EstimatedCost = 25m, Quantity = 2, RecommendedTiming = PurchaseTiming.AntesNascimento } },
                { "fraldas_pack", new BabyShoppingItem { Name = "Fraldas Recém-Nascido (pack)", Category = ShoppingCategory.HigieneBanho, Priority = ShoppingPriority.Essencial, EstimatedCost = 30m, Quantity = 3, RecommendedTiming = PurchaseTiming.AntesNascimento } },
                { "banheira", new BabyShoppingItem { Name = "Banheira para Bebé", Category = ShoppingCategory.HigieneBanho, Priority = ShoppingPriority.Essencial, EstimatedCost = 30m, Quantity = 1, RecommendedTiming = PurchaseTiming.Mes8 } },
                { "toalhas", new BabyShoppingItem { Name = "Toalhas com Capuz (pack 2)", Category = ShoppingCategory.HigieneBanho, Priority = ShoppingPriority.Essencial, EstimatedCost = 20m, Quantity = 1, RecommendedTiming = PurchaseTiming.Mes8 } },
                { "mala_maternidade", new BabyShoppingItem { Name = "Mala da Maternidade", Category = ShoppingCategory.Outros, Priority = ShoppingPriority.Essencial, EstimatedCost = 40m, Quantity = 1, RecommendedTiming = PurchaseTiming.Mes8 } }
            };
        }
    }
}
