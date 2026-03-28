using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;
using Ervilhinha.Services;
using System.Security.Claims;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class BabyCostSimulatorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BabyCostCalculatorService _calculatorService;

        public BabyCostSimulatorController(ApplicationDbContext context, BabyCostCalculatorService calculatorService)
        {
            _context = context;
            _calculatorService = calculatorService;
        }

        // GET: BabyCostSimulator
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var simulator = await _context.BabyCostSimulators
                .FirstOrDefaultAsync(s => s.UserId == userId!);

            if (simulator == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(simulator);
        }

        // GET: BabyCostSimulator/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var simulator = new BabyCostSimulator
            {
                UserId = userId!,
                MonthlyIncome = 1500m,
                Lifestyle = LifestyleLevel.Moderado,
                ExpectedBirthType = BirthType.PublicoNormal,
                HasGovernmentSupport = true,
                WillBreastfeed = true,
                HasDonatedItems = false,
                PregnancyWeeks = 20
            };

            return View(simulator);
        }

        // POST: BabyCostSimulator/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BabyCostSimulator simulator)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                simulator.UserId = userId!;

                // Calcular custos
                simulator = _calculatorService.CalculateCosts(simulator);

                _context.Add(simulator);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Simulação criada com sucesso! Vê as estimativas e recomendações abaixo.";
                return RedirectToAction(nameof(Index));
            }

            return View(simulator);
        }

        // GET: BabyCostSimulator/Edit
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var simulator = await _context.BabyCostSimulators
                .FirstOrDefaultAsync(s => s.UserId == userId!);

            if (simulator == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(simulator);
        }

        // POST: BabyCostSimulator/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BabyCostSimulator simulator)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.BabyCostSimulators
                        .FirstOrDefaultAsync(s => s.UserId == userId!);

                    if (existing == null)
                    {
                        return NotFound();
                    }

                    // Atualizar propriedades
                    existing.MonthlyIncome = simulator.MonthlyIncome;
                    existing.Lifestyle = simulator.Lifestyle;
                    existing.ExpectedBirthType = simulator.ExpectedBirthType;
                    existing.PregnancyWeeks = simulator.PregnancyWeeks;
                    existing.ExpectedDueDate = simulator.ExpectedDueDate;
                    existing.HasGovernmentSupport = simulator.HasGovernmentSupport;
                    existing.WillBreastfeed = simulator.WillBreastfeed;
                    existing.HasDonatedItems = simulator.HasDonatedItems;

                    // Recalcular custos
                    existing = _calculatorService.CalculateCosts(existing);

                    _context.Update(existing);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Simulação atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            return View(simulator);
        }

        // GET: BabyCostSimulator/SavingsGoal
        public async Task<IActionResult> SavingsGoal()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var simulator = await _context.BabyCostSimulators
                .FirstOrDefaultAsync(s => s.UserId == userId!);

            if (simulator == null)
            {
                TempData["Error"] = "Primeiro cria a tua simulação de custos.";
                return RedirectToAction(nameof(Create));
            }

            ViewBag.Simulator = simulator;
            return View();
        }

        // POST: BabyCostSimulator/CalculateSavings
        [HttpPost]
        public IActionResult CalculateSavings(decimal currentSavings, decimal monthlyIncome)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var simulator = _context.BabyCostSimulators
                .FirstOrDefault(s => s.UserId == userId!);

            if (simulator == null)
            {
                return Json(new { success = false, message = "Simulação não encontrada." });
            }

            var targetAmount = simulator.EstimatedTotalFirstYear;
            var remaining = _calculatorService.CalculateSavingsGoal(currentSavings, targetAmount);

            var monthsUntilBirth = simulator.ExpectedDueDate.HasValue 
                ? Math.Max(1, (int)((simulator.ExpectedDueDate.Value - DateTime.Today).TotalDays / 30))
                : 6;

            var recommendedMonthlySavings = _calculatorService.RecommendMonthlySavings(
                monthlyIncome, 
                targetAmount, 
                monthsUntilBirth
            );

            var monthsToGoal = _calculatorService.CalculateMonthsToGoal(
                currentSavings, 
                targetAmount, 
                recommendedMonthlySavings
            );

            return Json(new
            {
                success = true,
                targetAmount,
                currentSavings,
                remaining,
                recommendedMonthlySavings,
                monthsToGoal,
                monthsUntilBirth
            });
        }
    }
}
