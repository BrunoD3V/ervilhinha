using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;
using Ervilhinha.Helpers;
using System.Security.Claims;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class BabyListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BabyListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BabyLists
        public async Task<IActionResult> Index(ListType? type, bool? shared)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var lists = _context.BabyLists
                .Include(l => l.Items)
                .Include(l => l.Managers)
                .Where(l => l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail))
                .AsQueryable();

            if (type.HasValue)
            {
                lists = lists.Where(l => l.Type == type.Value);
            }

            if (shared.HasValue)
            {
                lists = lists.Where(l => l.IsShared == shared.Value);
            }

            var listData = await lists.OrderByDescending(l => l.CreatedDate).ToListAsync();

            ViewBag.TotalLists = listData.Count;
            ViewBag.SharedLists = listData.Count(l => l.IsShared);
            ViewBag.PrivateLists = listData.Count(l => !l.IsShared);

            return View(listData);
        }

        // GET: BabyLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BabyLists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BabyList babyList, string? coManagerEmail)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;
            var userName = userEmail.Split('@')[0];

            if (ModelState.IsValid)
            {
                babyList.UserId = userId!;
                babyList.CreatedBy = userEmail;
                babyList.CreatedDate = DateTime.UtcNow;

                // Se for lista partilhada, gerar ShareCode
                if (babyList.IsShared)
                {
                    string shareCode;
                    do
                    {
                        shareCode = ShareCodeGenerator.Generate();
                    } while (await _context.BabyLists.AnyAsync(l => l.ShareCode == shareCode));

                    babyList.ShareCode = shareCode;

                    // Adicionar criador como gestor
                    var creatorManager = new BabyListManager
                    {
                        BabyList = babyList,
                        ManagerEmail = userEmail,
                        ManagerName = userName,
                        Role = "Criador",
                        CanManageManagers = true,
                        AddedDate = DateTime.UtcNow
                    };
                    _context.BabyListManagers.Add(creatorManager);

                    // Adicionar co-gestor se fornecido
                    if (!string.IsNullOrWhiteSpace(coManagerEmail) && coManagerEmail != userEmail)
                    {
                        var coManager = new BabyListManager
                        {
                            BabyList = babyList,
                            ManagerEmail = coManagerEmail.Trim(),
                            ManagerName = coManagerEmail.Split('@')[0],
                            Role = "Co-Gestor",
                            CanManageManagers = true,
                            AddedDate = DateTime.UtcNow
                        };
                        _context.BabyListManagers.Add(coManager);
                    }
                }

                _context.Add(babyList);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"✅ Lista '{babyList.Name}' criada com sucesso!";
                if (babyList.IsShared)
                {
                    TempData["ShareCode"] = babyList.ShareCode;
                }

                return RedirectToAction(nameof(Manage), new { id = babyList.Id });
            }

            return View(babyList);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartial([FromForm] BabyList babyList)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userEmail = User.Identity?.Name ?? string.Empty;

                if (ModelState.IsValid)
                {
                    babyList.UserId = userId!;
                    babyList.CreatedBy = userEmail;
                    babyList.CreatedDate = DateTime.UtcNow;

                    _context.Add(babyList);
                    await _context.SaveChangesAsync();

                    return PartialView("_ListCard", babyList);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: BabyLists/Manage/5
        public async Task<IActionResult> Manage(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var babyList = await _context.BabyLists
                .Include(l => l.Items)
                    .ThenInclude(i => i.Reservations)
                .Include(l => l.Managers)
                .Include(l => l.Shares)
                .FirstOrDefaultAsync(l => l.Id == id && 
                    (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (babyList == null) return NotFound();

            return View(babyList);
        }

        // GET: BabyLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var babyList = await _context.BabyLists
                .Include(l => l.Managers)
                .FirstOrDefaultAsync(l => l.Id == id && 
                    (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (babyList == null) return NotFound();

            return View(babyList);
        }

        // POST: BabyLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BabyList babyList)
        {
            if (id != babyList.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var existingList = await _context.BabyLists
                .Include(l => l.Managers)
                .FirstOrDefaultAsync(l => l.Id == id && 
                    (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (existingList == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingList.Name = babyList.Name;
                existingList.Description = babyList.Description;
                existingList.Type = babyList.Type;
                existingList.ExpectedDate = babyList.ExpectedDate;
                existingList.BabyName = babyList.BabyName;
                existingList.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                TempData["Success"] = "✅ Lista atualizada com sucesso!";
                return RedirectToAction(nameof(Manage), new { id });
            }

            return View(babyList);
        }

        // POST: BabyLists/ToggleSharing/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSharing(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;
            var userName = userEmail.Split('@')[0];

            var babyList = await _context.BabyLists
                .Include(l => l.Managers)
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);

            if (babyList == null) return NotFound();

            if (!babyList.IsShared)
            {
                // Ativar partilha
                string shareCode;
                do
                {
                    shareCode = ShareCodeGenerator.Generate();
                } while (await _context.BabyLists.AnyAsync(l => l.ShareCode == shareCode));

                babyList.ShareCode = shareCode;
                babyList.IsShared = true;

                // Adicionar criador como gestor se ainda não existir
                if (!babyList.Managers.Any())
                {
                    var creatorManager = new BabyListManager
                    {
                        BabyListId = babyList.Id,
                        ManagerEmail = userEmail,
                        ManagerName = userName,
                        Role = "Criador",
                        CanManageManagers = true,
                        AddedDate = DateTime.UtcNow
                    };
                    _context.BabyListManagers.Add(creatorManager);
                }

                TempData["Success"] = $"✅ Partilha ativada! Código: {shareCode}";
            }
            else
            {
                // Desativar partilha
                babyList.IsShared = false;
                babyList.IsPublic = false;
                TempData["Success"] = "Lista agora é privada";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage), new { id });
        }

        // POST: BabyLists/TogglePublic/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublic(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var babyList = await _context.BabyLists
                .Include(l => l.Managers)
                .FirstOrDefaultAsync(l => l.Id == id && 
                    (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (babyList == null) return NotFound();

            if (!babyList.IsShared)
            {
                TempData["Error"] = "❌ A lista precisa estar com partilha ativada primeiro!";
                return RedirectToAction(nameof(Manage), new { id });
            }

            babyList.IsPublic = !babyList.IsPublic;
            await _context.SaveChangesAsync();

            TempData["Success"] = babyList.IsPublic 
                ? "🔓 Lista agora é pública (qualquer pessoa com o link pode ver)" 
                : "🔒 Lista agora é privada (apenas convidados podem ver)";

            return RedirectToAction(nameof(Manage), new { id });
        }

        // GET: BabyLists/AddItem/5
        public async Task<IActionResult> AddItem(int? listId)
        {
            if (listId == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var babyList = await _context.BabyLists
                .Include(l => l.Managers)
                .FirstOrDefaultAsync(l => l.Id == listId && 
                    (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (babyList == null) return NotFound();

            ViewBag.ListId = listId;
            ViewBag.ListName = babyList.Name;
            return View();
        }

        // POST: BabyLists/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(BabyListItem item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var babyList = await _context.BabyLists
                .Include(l => l.Managers)
                .FirstOrDefaultAsync(l => l.Id == item.BabyListId && 
                    (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (babyList == null) return NotFound();

            if (ModelState.IsValid)
            {
                item.CreatedDate = DateTime.UtcNow;
                item.CreatedBy = userEmail;

                _context.Add(item);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"✅ Item '{item.Name}' adicionado!";
                return RedirectToAction(nameof(Manage), new { id = item.BabyListId });
            }

            ViewBag.ListId = item.BabyListId;
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemAjax([FromForm] BabyListItem item)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userEmail = User.Identity?.Name ?? string.Empty;

                var babyList = await _context.BabyLists
                    .Include(l => l.Managers)
                    .FirstOrDefaultAsync(l => l.Id == item.BabyListId && 
                        (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

                if (babyList == null) 
                    return Json(new { success = false, message = "Lista não encontrada" });

                if (ModelState.IsValid)
                {
                    item.CreatedDate = DateTime.UtcNow;
                    item.CreatedBy = userEmail;

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
        public async Task<IActionResult> AddItemPartial([FromForm] BabyListItem item)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userEmail = User.Identity?.Name ?? string.Empty;

                var babyList = await _context.BabyLists
                    .Include(l => l.Managers)
                    .FirstOrDefaultAsync(l => l.Id == item.BabyListId && 
                        (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

                if (babyList == null) 
                    return BadRequest("Lista não encontrada");

                if (ModelState.IsValid)
                {
                    item.CreatedDate = DateTime.UtcNow;
                    item.CreatedBy = userEmail;

                    _context.Add(item);
                    await _context.SaveChangesAsync();

                    return PartialView("_ItemRow", item);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: BabyLists/EditItem/5
        public async Task<IActionResult> EditItem(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var item = await _context.BabyListItems
                .Include(i => i.BabyList)
                    .ThenInclude(l => l.Managers)
                .FirstOrDefaultAsync(i => i.Id == id && 
                    (i.BabyList.UserId == userId || i.BabyList.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (item == null) return NotFound();

            return View(item);
        }

        // POST: BabyLists/EditItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, BabyListItem item)
        {
            if (id != item.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var existingItem = await _context.BabyListItems
                .Include(i => i.BabyList)
                    .ThenInclude(l => l.Managers)
                .FirstOrDefaultAsync(i => i.Id == id && 
                    (i.BabyList.UserId == userId || i.BabyList.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (existingItem == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingItem.Name = item.Name;
                existingItem.Description = item.Description;
                existingItem.Category = item.Category;
                existingItem.Priority = item.Priority;
                existingItem.EstimatedCost = item.EstimatedCost;
                existingItem.ActualCost = item.ActualCost;
                existingItem.Quantity = item.Quantity;
                existingItem.RecommendedTiming = item.RecommendedTiming;
                existingItem.IsPurchased = item.IsPurchased;
                existingItem.PurchaseDate = item.PurchaseDate;
                existingItem.IsGift = item.IsGift;
                existingItem.ProductUrl = item.ProductUrl;
                existingItem.ImageUrl = item.ImageUrl;
                existingItem.Notes = item.Notes;

                await _context.SaveChangesAsync();

                TempData["Success"] = "✅ Item atualizado!";
                return RedirectToAction(nameof(Manage), new { id = existingItem.BabyListId });
            }

            return View(item);
        }

        // POST: BabyLists/DeleteItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var item = await _context.BabyListItems
                .Include(i => i.BabyList)
                    .ThenInclude(l => l.Managers)
                .FirstOrDefaultAsync(i => i.Id == id && 
                    (i.BabyList.UserId == userId || i.BabyList.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (item == null) return NotFound();

            var listId = item.BabyListId;

            _context.BabyListItems.Remove(item);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ Item removido!";
            return RedirectToAction(nameof(Manage), new { id = listId });
        }

        // POST: BabyLists/TogglePurchased/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePurchased(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var item = await _context.BabyListItems
                .Include(i => i.BabyList)
                    .ThenInclude(l => l.Managers)
                .FirstOrDefaultAsync(i => i.Id == id && 
                    (i.BabyList.UserId == userId || i.BabyList.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (item == null) return NotFound();

            item.IsPurchased = !item.IsPurchased;
            
            if (item.IsPurchased)
            {
                item.PurchaseDate = DateTime.Now;
                item.AcquiredQuantity = item.Quantity;
            }
            else
            {
                item.PurchaseDate = null;
                item.AcquiredQuantity = 0;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = item.IsPurchased 
                ? "✅ Marcado como comprado!" 
                : "🔄 Desmarcado";

            return RedirectToAction(nameof(Manage), new { id = item.BabyListId });
        }

        // GET: BabyLists/Share/5
        public async Task<IActionResult> Share(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name ?? string.Empty;

            var babyList = await _context.BabyLists
                .Include(l => l.Managers)
                .Include(l => l.Shares)
                .FirstOrDefaultAsync(l => l.Id == id && 
                    (l.UserId == userId || l.Managers.Any(m => m.ManagerEmail == userEmail)));

            if (babyList == null) return NotFound();

            if (!babyList.IsShared)
            {
                TempData["Error"] = "❌ A lista precisa estar com partilha ativada!";
                return RedirectToAction(nameof(Manage), new { id });
            }

            return View(babyList);
        }

        // GET: BabyLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var babyList = await _context.BabyLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);

            if (babyList == null) return NotFound();

            return View(babyList);
        }

        // POST: BabyLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var babyList = await _context.BabyLists
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);

            if (babyList == null) return NotFound();

            _context.BabyLists.Remove(babyList);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ Lista eliminada!";
            return RedirectToAction(nameof(Index));
        }
    }
}
