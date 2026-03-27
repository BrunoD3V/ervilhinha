using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;
using Ervilhinha.Helpers;

namespace Ervilhinha.Controllers
{
    /// <summary>
    /// Controller para visualização pública de listas de desejos (sem login)
    /// </summary>
    public class WishlistPublicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistPublicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /WishlistPublic/View/ABC-DEF-GHI
        public async Task<IActionResult> View(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return NotFound();

            // Normalizar código
            code = ShareCodeGenerator.Normalize(code);

            var wishlist = await _context.BabyWishlists
                .Include(w => w.Items)
                    .ThenInclude(i => i.Reservations)
                .Include(w => w.Managers)
                .FirstOrDefaultAsync(w => w.ShareCode == code);

            if (wishlist == null)
            {
                ViewBag.ErrorMessage = "Lista não encontrada. Verifica o código e tenta novamente.";
                return base.View("NotFound");
            }

            // Verificar se é pública ou se user tem acesso
            if (!wishlist.IsPublic)
            {
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    ViewBag.ErrorMessage = "Esta lista é privada. Por favor, inicia sessão.";
                    return base.View("Private");
                }

                // Verificar se é gestor ou convidado
                var isManager = wishlist.Managers.Any(m => m.ManagerEmail == userEmail);
                var isInvited = await _context.WishlistShares
                    .AnyAsync(s => s.WishlistId == wishlist.Id && s.SharedWithEmail == userEmail);

                if (!isManager && !isInvited)
                {
                    ViewBag.ErrorMessage = "Não tens permissão para ver esta lista.";
                    return base.View("Forbidden");
                }
            }

            // Calcular estatísticas
            ViewBag.TotalItems = wishlist.Items.Count;
            ViewBag.AvailableItems = wishlist.Items.Count(i => i.AvailableQuantity > 0);
            ViewBag.ReservedItems = wishlist.Items.Count(i => i.ReservedQuantity > 0);
            ViewBag.AcquiredItems = wishlist.Items.Count(i => i.AcquiredQuantity > 0);

            return base.View(wishlist);
        }

        // GET: /WishlistPublic/Reserve/5
        public async Task<IActionResult> Reserve(int id)
        {
            var item = await _context.WishlistItems
                .Include(i => i.Wishlist)
                .Include(i => i.Reservations)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return NotFound();

            if (item.AvailableQuantity <= 0)
            {
                TempData["ErrorMessage"] = "❌ Este item já está totalmente reservado.";
                return RedirectToAction(nameof(View), new { code = item.Wishlist.ShareCode });
            }

            ViewBag.WishlistCode = item.Wishlist.ShareCode;
            ViewBag.WishlistName = item.Wishlist.Name;
            ViewBag.MaxQuantity = item.AvailableQuantity;

            return base.View(item);
        }

        // POST: /WishlistPublic/Reserve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int itemId, string reservedByName, string reservedByEmail, int quantity, string? message)
        {
            var item = await _context.WishlistItems
                .Include(i => i.Wishlist)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null)
                return NotFound();

            // Validações
            if (string.IsNullOrWhiteSpace(reservedByName) || string.IsNullOrWhiteSpace(reservedByEmail))
            {
                TempData["ErrorMessage"] = "❌ Nome e email são obrigatórios.";
                return RedirectToAction(nameof(Reserve), new { id = itemId });
            }

            if (quantity <= 0 || quantity > item.AvailableQuantity)
            {
                TempData["ErrorMessage"] = $"❌ Quantidade inválida. Disponível: {item.AvailableQuantity}";
                return RedirectToAction(nameof(Reserve), new { id = itemId });
            }

            // Verificar se já reservou este item
            var existingReservation = await _context.WishlistReservations
                .FirstOrDefaultAsync(r => r.WishlistItemId == itemId && r.ReservedBy == reservedByEmail);

            if (existingReservation != null)
            {
                TempData["ErrorMessage"] = "❌ Já reservaste este item anteriormente.";
                return RedirectToAction(nameof(View), new { code = item.Wishlist.ShareCode });
            }

            // Criar reserva
            var reservation = new WishlistReservation
            {
                WishlistItemId = itemId,
                ReservedBy = reservedByEmail,
                ReservedByName = reservedByName,
                Quantity = quantity,
                Message = message,
                ReservedDate = DateTime.UtcNow
            };

            _context.WishlistReservations.Add(reservation);

            // Atualizar quantidade reservada
            item.ReservedQuantity += quantity;

            await _context.SaveChangesAsync();

            // TODO: Enviar notificação aos pais

            TempData["SuccessMessage"] = $"🎁 Obrigado! Reservaste {quantity}x '{item.Name}'!";
            return RedirectToAction(nameof(Confirmation), new { id = reservation.Id });
        }

        // GET: /WishlistPublic/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var reservation = await _context.WishlistReservations
                .Include(r => r.WishlistItem)
                    .ThenInclude(i => i.Wishlist)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return base.View(reservation);
        }

        // GET: /WishlistPublic/MyReservations?email=...
        public async Task<IActionResult> MyReservations(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return base.View("EnterEmail");
            }

            var reservations = await _context.WishlistReservations
                .Include(r => r.WishlistItem)
                    .ThenInclude(i => i.Wishlist)
                .Where(r => r.ReservedBy == email)
                .OrderByDescending(r => r.ReservedDate)
                .ToListAsync();

            ViewBag.Email = email;
            return base.View(reservations);
        }

        // POST: /WishlistPublic/CancelReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int id, string email)
        {
            var reservation = await _context.WishlistReservations
                .Include(r => r.WishlistItem)
                    .ThenInclude(i => i.Wishlist)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            // Verificar se o email corresponde
            if (reservation.ReservedBy != email)
            {
                TempData["ErrorMessage"] = "❌ Não podes cancelar esta reserva.";
                return RedirectToAction(nameof(MyReservations), new { email });
            }

            // Não permitir cancelar se já foi entregue
            if (reservation.IsDelivered)
            {
                TempData["ErrorMessage"] = "❌ Não podes cancelar uma reserva já entregue.";
                return RedirectToAction(nameof(MyReservations), new { email });
            }

            // Devolver quantidade ao item
            var item = reservation.WishlistItem;
            item.ReservedQuantity -= reservation.Quantity;

            _context.WishlistReservations.Remove(reservation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "✅ Reserva cancelada com sucesso!";
            return RedirectToAction(nameof(MyReservations), new { email });
        }

        // GET: /WishlistPublic/AccessDenied
        public IActionResult AccessDenied()
        {
            return base.View();
        }
    }
}
