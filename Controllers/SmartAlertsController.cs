using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ervilhinha.Services;
using System.Security.Claims;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class SmartAlertsController : Controller
    {
        private readonly SmartAlertService _alertService;

        public SmartAlertsController(SmartAlertService alertService)
        {
            _alertService = alertService;
        }

        // GET: SmartAlerts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alerts = await _alertService.GetActiveAlerts(userId!, 50);

            return View(alerts);
        }

        // POST: SmartAlerts/MarkAsRead/5
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _alertService.MarkAsRead(id);
            return Json(new { success = true });
        }

        // POST: SmartAlerts/Dismiss/5
        [HttpPost]
        public async Task<IActionResult> Dismiss(int id)
        {
            await _alertService.DismissAlert(id);
            TempData["Success"] = "Alerta dispensado.";
            return RedirectToAction(nameof(Index));
        }

        // POST: SmartAlerts/DismissAll
        [HttpPost]
        public async Task<IActionResult> DismissAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alerts = await _alertService.GetActiveAlerts(userId!, 100);

            foreach (var alert in alerts)
            {
                await _alertService.DismissAlert(alert.Id);
            }

            TempData["Success"] = "Todos os alertas foram dispensados.";
            return RedirectToAction(nameof(Index));
        }
    }
}
