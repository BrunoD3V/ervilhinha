using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;
using Ervilhinha.Helpers;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Wishlist/MyLists
        public async Task<IActionResult> MyLists()
        {
            var userEmail = User.Identity?.Name ?? string.Empty;

            // Listas onde sou criador ou gestor
            var myLists = await _context.BabyWishlists
                .Include(w => w.Managers)
                .Include(w => w.Items)
                .Where(w => w.CreatedBy == userEmail || 
                           w.Managers.Any(m => m.ManagerEmail == userEmail))
                .OrderByDescending(w => w.CreatedDate)
                .ToListAsync();

            return View(myLists);
        }

        // GET: /Wishlist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Wishlist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BabyWishlist wishlist, string? coManagerEmail)
        {
            var userEmail = User.Identity?.Name ?? string.Empty;
            var userName = User.Identity?.Name?.Split('@')[0] ?? "Utilizador";

            if (ModelState.IsValid)
            {
                // Gerar ShareCode único
                string shareCode;
                do
                {
                    shareCode = ShareCodeGenerator.Generate();
                } while (await _context.BabyWishlists.AnyAsync(w => w.ShareCode == shareCode));

                wishlist.ShareCode = shareCode;
                wishlist.CreatedBy = userEmail;
                wishlist.CreatedDate = DateTime.UtcNow;

                _context.Add(wishlist);

                // Adicionar criador como gestor
                var creatorManager = new WishlistManager
                {
                    Wishlist = wishlist,
                    ManagerEmail = userEmail,
                    ManagerName = userName,
                    Role = "Criador",
                    CanManageManagers = true,
                    AddedDate = DateTime.UtcNow
                };
                _context.WishlistManagers.Add(creatorManager);

                // Adicionar co-gestor se fornecido
                if (!string.IsNullOrWhiteSpace(coManagerEmail) && coManagerEmail != userEmail)
                {
                    var coManager = new WishlistManager
                    {
                        Wishlist = wishlist,
                        ManagerEmail = coManagerEmail.Trim(),
                        ManagerName = coManagerEmail.Split('@')[0],
                        Role = "Co-Gestor",
                        CanManageManagers = true,
                        AddedDate = DateTime.UtcNow
                    };
                    _context.WishlistManagers.Add(coManager);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"✅ Lista '{wishlist.Name}' criada com sucesso! Código de partilha: {shareCode}";
                return RedirectToAction(nameof(Manage), new { id = wishlist.Id });
            }

            return View(wishlist);
        }

        // GET: /Wishlist/Manage/5
        public async Task<IActionResult> Manage(int id)
        {
            var userEmail = User.Identity?.Name ?? string.Empty;

            var wishlist = await _context.BabyWishlists
                .Include(w => w.Managers)
                .Include(w => w.Items)
                    .ThenInclude(i => i.Reservations)
                .Include(w => w.Shares)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wishlist == null)
                return NotFound();

            // Verificar permissões
            if (!IsManager(wishlist, userEmail))
            {
                TempData["ErrorMessage"] = "❌ Não tens permissão para gerir esta lista.";
                return RedirectToAction(nameof(MyLists));
            }

            return View(wishlist);
        }

        // GET: /Wishlist/AddItem/5
        public async Task<IActionResult> AddItem(int id)
        {
            var wishlist = await _context.BabyWishlists.FindAsync(id);
            if (wishlist == null)
                return NotFound();

            var item = new WishlistItem { WishlistId = id };
            ViewBag.WishlistName = wishlist.Name;
            
            return View(item);
        }

        // POST: /Wishlist/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(WishlistItem item)
        {
            var userEmail = User.Identity?.Name ?? string.Empty;

            if (ModelState.IsValid)
            {
                item.CreatedBy = userEmail;
                item.CreatedDate = DateTime.UtcNow;

                _context.WishlistItems.Add(item);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"✅ Item '{item.Name}' adicionado com sucesso!";
                return RedirectToAction(nameof(Manage), new { id = item.WishlistId });
            }

            var wishlist = await _context.BabyWishlists.FindAsync(item.WishlistId);
            ViewBag.WishlistName = wishlist?.Name;
            
            return View(item);
        }

        // GET: /Wishlist/EditItem/5
        public async Task<IActionResult> EditItem(int id)
        {
            var item = await _context.WishlistItems
                .Include(i => i.Wishlist)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return NotFound();

            ViewBag.WishlistName = item.Wishlist.Name;
            return View(item);
        }

        // POST: /Wishlist/EditItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, WishlistItem item)
        {
            if (id != item.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingItem = await _context.WishlistItems.FindAsync(id);
                    if (existingItem == null)
                        return NotFound();

                    // Atualizar apenas campos editáveis
                    existingItem.Name = item.Name;
                    existingItem.Description = item.Description;
                    existingItem.Category = item.Category;
                    existingItem.Priority = item.Priority;
                    existingItem.EstimatedPrice = item.EstimatedPrice;
                    existingItem.Quantity = item.Quantity;
                    existingItem.ProductUrl = item.ProductUrl;
                    existingItem.ImageUrl = item.ImageUrl;
                    existingItem.Notes = item.Notes;

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "✅ Item atualizado com sucesso!";
                    return RedirectToAction(nameof(Manage), new { id = existingItem.WishlistId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            var wishlist = await _context.BabyWishlists.FindAsync(item.WishlistId);
            ViewBag.WishlistName = wishlist?.Name;
            
            return View(item);
        }

        // POST: /Wishlist/DeleteItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.WishlistItems.FindAsync(id);
            if (item == null)
                return NotFound();

            var wishlistId = item.WishlistId;

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "✅ Item removido com sucesso!";
            return RedirectToAction(nameof(Manage), new { id = wishlistId });
        }

        // POST: /Wishlist/MarkAcquired
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAcquired(int id, int quantity)
        {
            var item = await _context.WishlistItems.FindAsync(id);
            if (item == null)
                return NotFound();

            item.AcquiredQuantity += quantity;
            if (item.AcquiredQuantity > item.Quantity)
                item.AcquiredQuantity = item.Quantity;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"✅ {quantity}x '{item.Name}' marcado como adquirido!";
            return RedirectToAction(nameof(Manage), new { id = item.WishlistId });
        }

        // GET: /Wishlist/Share/5
        public async Task<IActionResult> Share(int id)
        {
            var wishlist = await _context.BabyWishlists
                .Include(w => w.Shares)
                .Include(w => w.Managers)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wishlist == null)
                return NotFound();

            var userEmail = User.Identity?.Name ?? string.Empty;
            if (!IsManager(wishlist, userEmail))
            {
                TempData["ErrorMessage"] = "❌ Não tens permissão para partilhar esta lista.";
                return RedirectToAction(nameof(MyLists));
            }

            ViewBag.ShareUrl = Url.Action("View", "WishlistPublic", 
                new { code = wishlist.ShareCode }, Request.Scheme);

            return View(wishlist);
        }

        // POST: /Wishlist/InviteByEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteByEmail(int wishlistId, string email, string? name, string? message)
        {
            var userEmail = User.Identity?.Name ?? string.Empty;
            var wishlist = await _context.BabyWishlists.FindAsync(wishlistId);

            if (wishlist == null)
                return NotFound();

            // Verificar se já foi convidado
            var existingShare = await _context.WishlistShares
                .FirstOrDefaultAsync(s => s.WishlistId == wishlistId && s.SharedWithEmail == email);

            if (existingShare != null)
            {
                TempData["ErrorMessage"] = $"❌ {email} já foi convidado anteriormente.";
                return RedirectToAction(nameof(Share), new { id = wishlistId });
            }

            var share = new WishlistShare
            {
                WishlistId = wishlistId,
                SharedWithEmail = email,
                SharedWithName = name,
                InviteMessage = message,
                SharedBy = userEmail,
                SharedDate = DateTime.UtcNow,
                Permission = "Reserve"
            };

            _context.WishlistShares.Add(share);
            await _context.SaveChangesAsync();

            // TODO: Enviar email com convite

            TempData["SuccessMessage"] = $"✅ Convite enviado para {email}!";
            return RedirectToAction(nameof(Share), new { id = wishlistId });
        }

        // POST: /Wishlist/TogglePublic/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublic(int id)
        {
            var wishlist = await _context.BabyWishlists.FindAsync(id);
            if (wishlist == null)
                return NotFound();

            wishlist.IsPublic = !wishlist.IsPublic;
            await _context.SaveChangesAsync();

            var status = wishlist.IsPublic ? "pública" : "privada";
            TempData["SuccessMessage"] = $"✅ Lista agora é {status}!";
            
            return RedirectToAction(nameof(Share), new { id });
        }

        // POST: /Wishlist/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userEmail = User.Identity?.Name ?? string.Empty;
            var wishlist = await _context.BabyWishlists
                .Include(w => w.Managers)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wishlist == null)
                return NotFound();

            // Apenas o criador pode eliminar
            if (wishlist.CreatedBy != userEmail)
            {
                TempData["ErrorMessage"] = "❌ Apenas o criador pode eliminar a lista.";
                return RedirectToAction(nameof(MyLists));
            }

            _context.BabyWishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"✅ Lista '{wishlist.Name}' eliminada com sucesso!";
            return RedirectToAction(nameof(MyLists));
        }

        #region Helpers

        private bool IsManager(BabyWishlist wishlist, string email)
        {
            return wishlist.CreatedBy == email || 
                   wishlist.Managers.Any(m => m.ManagerEmail == email);
        }

        #endregion
    }
}
