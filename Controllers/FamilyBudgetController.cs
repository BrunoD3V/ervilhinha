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
    public class FamilyBudgetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FamilyBudgetService _budgetService;
        private readonly SmartAlertService _alertService;

        public FamilyBudgetController(
            ApplicationDbContext context, 
            FamilyBudgetService budgetService,
            SmartAlertService alertService)
        {
            _context = context;
            _budgetService = budgetService;
            _alertService = alertService;
        }

        // GET: FamilyBudget
        public async Task<IActionResult> Index(int? year, int? month)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var selectedYear = year ?? DateTime.Today.Year;
            var selectedMonth = month ?? DateTime.Today.Month;

            // Atualizar despesas reais do mês
            var budget = await _budgetService.UpdateActualExpenses(userId!, selectedYear, selectedMonth);

            // Gerar alertas
            await _alertService.GenerateBudgetAlerts(userId!, budget);

            ViewBag.Year = selectedYear;
            ViewBag.Month = selectedMonth;
            ViewBag.Months = Enumerable.Range(1, 12).Select(m => new { Value = m, Text = new DateTime(2000, m, 1).ToString("MMMM", new System.Globalization.CultureInfo("pt-PT")) });
            ViewBag.Years = Enumerable.Range(DateTime.Today.Year - 2, 5);

            return View(budget);
        }

        // GET: FamilyBudget/Edit
        public async Task<IActionResult> Edit(int? year, int? month)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var selectedYear = year ?? DateTime.Today.Year;
            var selectedMonth = month ?? DateTime.Today.Month;

            var budget = await _context.FamilyBudgets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Year == selectedYear && b.Month == selectedMonth);

            if (budget == null)
            {
                budget = new FamilyBudget
                {
                    UserId = userId!,
                    Year = selectedYear,
                    Month = selectedMonth,
                    TotalIncome = 0
                };
            }

            ViewBag.Year = selectedYear;
            ViewBag.Month = selectedMonth;
            ViewBag.MonthName = new DateTime(selectedYear, selectedMonth, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("pt-PT"));

            return View(budget);
        }

        // POST: FamilyBudget/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FamilyBudget budget)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var existing = await _context.FamilyBudgets
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.Year == budget.Year && b.Month == budget.Month);

                if (existing == null)
                {
                    budget.UserId = userId!;
                    budget.CreatedDate = DateTime.UtcNow;
                    _context.Add(budget);
                }
                else
                {
                    existing.TotalIncome = budget.TotalIncome;
                    existing.BudgetBaby = budget.BudgetBaby;
                    existing.BudgetHouse = budget.BudgetHouse;
                    existing.BudgetFood = budget.BudgetFood;
                    existing.BudgetTransport = budget.BudgetTransport;
                    existing.BudgetHealth = budget.BudgetHealth;
                    existing.BudgetLeisure = budget.BudgetLeisure;
                    existing.BudgetSavings = budget.BudgetSavings;
                    existing.BudgetOther = budget.BudgetOther;
                    existing.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(existing);
                }

                await _context.SaveChangesAsync();

                // Atualizar despesas reais
                await _budgetService.UpdateActualExpenses(userId!, budget.Year, budget.Month);

                TempData["Success"] = "Orçamento guardado com sucesso!";
                return RedirectToAction(nameof(Index), new { year = budget.Year, month = budget.Month });
            }

            ViewBag.Year = budget.Year;
            ViewBag.Month = budget.Month;
            ViewBag.MonthName = new DateTime(budget.Year, budget.Month, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("pt-PT"));

            return View(budget);
        }

        // GET: FamilyBudget/Recommend
        public IActionResult Recommend()
        {
            return View();
        }

        // POST: FamilyBudget/GenerateRecommendation
        [HttpPost]
        public IActionResult GenerateRecommendation(decimal monthlyIncome, bool hasBaby)
        {
            var recommendedBudget = _budgetService.RecommendBudget(monthlyIncome, hasBaby);

            return Json(new
            {
                success = true,
                budget = new
                {
                    totalIncome = recommendedBudget.TotalIncome,
                    budgetBaby = recommendedBudget.BudgetBaby,
                    budgetHouse = recommendedBudget.BudgetHouse,
                    budgetFood = recommendedBudget.BudgetFood,
                    budgetTransport = recommendedBudget.BudgetTransport,
                    budgetHealth = recommendedBudget.BudgetHealth,
                    budgetLeisure = recommendedBudget.BudgetLeisure,
                    budgetSavings = recommendedBudget.BudgetSavings,
                    budgetOther = recommendedBudget.BudgetOther
                }
            });
        }

        // GET: FamilyBudget/Analysis
        public async Task<IActionResult> Analysis()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var budgets = await _context.FamilyBudgets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => new DateTime(b.Year, b.Month, 1))
                .Take(12)
                .ToListAsync();

            return View(budgets);
        }

        // GET: FamilyBudget/Forecast
        public async Task<IActionResult> Forecast()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var forecastBaby = await _budgetService.ForecastNextMonth(userId!, "baby");
            var forecastHouse = await _budgetService.ForecastNextMonth(userId!, "house");
            var forecastFood = await _budgetService.ForecastNextMonth(userId!, "food");
            var forecastTransport = await _budgetService.ForecastNextMonth(userId!, "transport");
            var forecastHealth = await _budgetService.ForecastNextMonth(userId!, "health");
            var forecastLeisure = await _budgetService.ForecastNextMonth(userId!, "leisure");

            var totalForecast = forecastBaby + forecastHouse + forecastFood + 
                              forecastTransport + forecastHealth + forecastLeisure;

            ViewBag.ForecastBaby = forecastBaby;
            ViewBag.ForecastHouse = forecastHouse;
            ViewBag.ForecastFood = forecastFood;
            ViewBag.ForecastTransport = forecastTransport;
            ViewBag.ForecastHealth = forecastHealth;
            ViewBag.ForecastLeisure = forecastLeisure;
            ViewBag.TotalForecast = totalForecast;

            return View();
        }
    }
}
