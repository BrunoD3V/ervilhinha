using Ervilhinha.Models;
using Ervilhinha.Data;
using Ervilhinha.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Ervilhinha.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SmartAlertService _alertService;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            SmartAlertService alertService)
        {
            _logger = logger;
            _context = context;
            _alertService = alertService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Obter alertas ativos
                var alerts = await _alertService.GetActiveAlerts(userId!, 5);
                ViewBag.Alerts = alerts;

                // Obter simulador de custos
                var simulator = await _context.BabyCostSimulators
                    .FirstOrDefaultAsync(s => s.UserId == userId);
                ViewBag.Simulator = simulator;

                // Obter orçamento atual
                var currentBudget = await _context.FamilyBudgets
                    .FirstOrDefaultAsync(b => b.UserId == userId && 
                                            b.Year == DateTime.Today.Year && 
                                            b.Month == DateTime.Today.Month);
                ViewBag.CurrentBudget = currentBudget;

                // Próximos eventos da timeline
                var upcomingEvents = await _context.BabyTimelines
                    .Where(t => t.UserId == userId && !t.IsCompleted && t.EventDate.HasValue)
                    .Where(t => t.EventDate >= DateTime.Today)
                    .OrderBy(t => t.EventDate)
                    .Take(5)
                    .ToListAsync();
                ViewBag.UpcomingEvents = upcomingEvents;

                // Items pendentes de compra
                var pendingItems = await _context.BabyShoppingItems
                    .Where(s => s.UserId == userId && !s.IsPurchased && s.Priority == ShoppingPriority.Essencial)
                    .Take(5)
                    .ToListAsync();
                ViewBag.PendingItems = pendingItems;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
