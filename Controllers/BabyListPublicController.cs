using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;

namespace Ervilhinha.Controllers
{
    public class BabyListPublicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BabyListPublicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BabyListPublic/ViewList?code=ABC123
        public async Task<IActionResult> ViewList(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("NotFound");
            }

            var babyList = await _context.BabyLists
                .Include(l => l.Items)
                    .ThenInclude(i => i.Reservations)
                .Include(l => l.Managers)
                .FirstOrDefaultAsync(l => l.ShareCode == code && l.IsShared);

            if (babyList == null)
            {
                return View("NotFound");
            }

            if (!babyList.IsPublic)
            {
                return View("Private");
            }

            return View("View", babyList);
        }

        // GET: BabyListPublic/Reserve?code=ABC123&itemId=5
        public async Task<IActionResult> Reserve(string? code, int? itemId)
        {
            if (string.IsNullOrWhiteSpace(code) || itemId == null)
            {
                return View("NotFound");
            }

            var item = await _context.BabyListItems
                .Include(i => i.BabyList)
                .Include(i => i.Reservations)
                .FirstOrDefaultAsync(i => i.Id == itemId && 
                                        i.BabyList.ShareCode == code && 
                                        i.BabyList.IsShared);

            if (item == null || item.AvailableQuantity <= 0)
            {
                return View("NotFound");
            }

            ViewBag.ShareCode = code;
            return View(item);
        }

        // POST: BabyListPublic/Reserve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(string code, int itemId, string reservedByName, string reservedBy, int quantity, string? message)
        {
            var item = await _context.BabyListItems
                .Include(i => i.BabyList)
                .Include(i => i.Reservations)
                .FirstOrDefaultAsync(i => i.Id == itemId && 
                                        i.BabyList.ShareCode == code && 
                                        i.BabyList.IsShared);

            if (item == null)
            {
                return View("NotFound");
            }

            if (quantity > item.AvailableQuantity)
            {
                TempData["Error"] = "❌ Quantidade não disponível!";
                return RedirectToAction(nameof(ViewList), new { code });
            }

            var reservation = new BabyListReservation
            {
                BabyListItemId = itemId,
                ReservedBy = reservedBy,
                ReservedByName = reservedByName,
                Quantity = quantity,
                Message = message,
                ContactEmail = reservedBy,
                ReservedDate = DateTime.UtcNow
            };

            item.ReservedQuantity += quantity;

            _context.BabyListReservations.Add(reservation);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✅ Reserva confirmada! Obrigado, {reservedByName}!";
            return RedirectToAction(nameof(Confirmation), new { code, reservationId = reservation.Id });
        }

        // GET: BabyListPublic/Confirmation
        public async Task<IActionResult> Confirmation(string? code, int? reservationId)
        {
            if (string.IsNullOrWhiteSpace(code) || reservationId == null)
            {
                return View("NotFound");
            }

            var reservation = await _context.BabyListReservations
                .Include(r => r.BabyListItem)
                    .ThenInclude(i => i.BabyList)
                .FirstOrDefaultAsync(r => r.Id == reservationId && 
                                         r.BabyListItem.BabyList.ShareCode == code);

            if (reservation == null)
            {
                return View("NotFound");
            }

            ViewBag.ShareCode = code;
            return View(reservation);
        }

        // GET: BabyListPublic/MyReservations?email=...
        public async Task<IActionResult> MyReservations(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return View("EnterEmail");
            }

            var reservations = await _context.BabyListReservations
                .Include(r => r.BabyListItem)
                    .ThenInclude(i => i.BabyList)
                .Where(r => r.ReservedBy == email)
                .OrderByDescending(r => r.ReservedDate)
                .ToListAsync();

            ViewBag.Email = email;
            return View(reservations);
        }
    }
}
