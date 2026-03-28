using Ervilhinha.Models;
using Ervilhinha.Data;
using Microsoft.EntityFrameworkCore;

namespace Ervilhinha.Services
{
    /// <summary>
    /// Serviço de Análise e Gestão de Orçamento Familiar
    /// </summary>
    public class FamilyBudgetService
    {
        private readonly ApplicationDbContext _context;

        public FamilyBudgetService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Atualiza os gastos reais do orçamento baseado nas despesas do mês
        /// </summary>
        public async Task<FamilyBudget> UpdateActualExpenses(string userId, int year, int month)
        {
            var budget = await _context.FamilyBudgets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Year == year && b.Month == month);

            if (budget == null)
            {
                // Criar novo orçamento se não existir
                budget = new FamilyBudget
                {
                    UserId = userId,
                    Year = year,
                    Month = month,
                    CreatedDate = DateTime.UtcNow
                };
                _context.FamilyBudgets.Add(budget);
            }

            // Buscar despesas do mês
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var expenses = await _context.Expenses
                .Include(e => e.ExpenseCategory)
                .Where(e => e.CreatedBy == userId && e.Date >= startDate && e.Date <= endDate)
                .ToListAsync();

            // Categorizar despesas
            budget.ActualBaby = expenses
                .Where(e => e.ExpenseCategory != null && 
                           (e.ExpenseCategory.Name.Contains("Bebé") || 
                            e.ExpenseCategory.Name.Contains("Criança") ||
                            e.ExpenseCategory.Name.Contains("Gravidez")))
                .Sum(e => e.Amount);

            budget.ActualHouse = expenses
                .Where(e => e.ExpenseCategory != null && 
                           (e.ExpenseCategory.Name.Contains("Casa") || 
                            e.ExpenseCategory.Name.Contains("Renda") ||
                            e.ExpenseCategory.Name.Contains("Habitação")))
                .Sum(e => e.Amount);

            budget.ActualFood = expenses
                .Where(e => e.ExpenseCategory != null && 
                           (e.ExpenseCategory.Name.Contains("Alimentação") || 
                            e.ExpenseCategory.Name.Contains("Comida") ||
                            e.ExpenseCategory.Name.Contains("Supermercado")))
                .Sum(e => e.Amount);

            budget.ActualTransport = expenses
                .Where(e => e.ExpenseCategory != null && 
                           (e.ExpenseCategory.Name.Contains("Transporte") || 
                            e.ExpenseCategory.Name.Contains("Carro") ||
                            e.ExpenseCategory.Name.Contains("Combustível")))
                .Sum(e => e.Amount);

            budget.ActualHealth = expenses
                .Where(e => e.ExpenseCategory != null && 
                           (e.ExpenseCategory.Name.Contains("Saúde") || 
                            e.ExpenseCategory.Name.Contains("Médico") ||
                            e.ExpenseCategory.Name.Contains("Farmácia")))
                .Sum(e => e.Amount);

            budget.ActualLeisure = expenses
                .Where(e => e.ExpenseCategory != null && 
                           (e.ExpenseCategory.Name.Contains("Lazer") || 
                            e.ExpenseCategory.Name.Contains("Entretenimento") ||
                            e.ExpenseCategory.Name.Contains("Restaurante")))
                .Sum(e => e.Amount);

            budget.ActualOther = expenses
                .Where(e => e.ExpenseCategory != null && 
                           !IsInCategory(e.ExpenseCategory.Name))
                .Sum(e => e.Amount);

            budget.LastUpdatedDate = DateTime.UtcNow;

            // Calcular estado de saúde financeira
            budget.HealthStatus = CalculateFinancialHealth(budget);

            // Gerar alertas
            budget.AlertMessages = GenerateAlerts(budget);

            await _context.SaveChangesAsync();

            return budget;
        }

        private bool IsInCategory(string categoryName)
        {
            var categories = new[] { "Bebé", "Criança", "Gravidez", "Casa", "Renda", "Habitação", 
                                    "Alimentação", "Comida", "Supermercado", "Transporte", "Carro", 
                                    "Combustível", "Saúde", "Médico", "Farmácia", "Lazer", 
                                    "Entretenimento", "Restaurante" };

            return categories.Any(c => categoryName.Contains(c));
        }

        private FinancialHealth CalculateFinancialHealth(FamilyBudget budget)
        {
            if (budget.TotalIncome == 0) return FinancialHealth.Normal;

            var totalBudget = budget.BudgetBaby + budget.BudgetHouse + budget.BudgetFood + 
                            budget.BudgetTransport + budget.BudgetHealth + budget.BudgetLeisure + 
                            budget.BudgetSavings + budget.BudgetOther;

            var totalActual = budget.ActualBaby + budget.ActualHouse + budget.ActualFood + 
                            budget.ActualTransport + budget.ActualHealth + budget.ActualLeisure + 
                            budget.ActualOther;

            // Percentagem do rendimento gasto
            var spentPercentage = (totalActual / budget.TotalIncome) * 100;

            // Percentagem de excesso face ao orçamento
            var budgetExcess = totalBudget > 0 ? ((totalActual - totalBudget) / totalBudget) * 100 : 0;

            // Poupanças alcançadas
            var savings = budget.TotalIncome - totalActual;
            var savingsPercentage = (savings / budget.TotalIncome) * 100;

            if (savingsPercentage >= 20 && budgetExcess <= 0)
                return FinancialHealth.Excelente;

            if (savingsPercentage >= 10 && budgetExcess <= 10)
                return FinancialHealth.Bom;

            if (spentPercentage <= 90 && budgetExcess <= 20)
                return FinancialHealth.Normal;

            if (spentPercentage > 90 || budgetExcess > 20)
                return FinancialHealth.Atencao;

            if (spentPercentage > 100 || savings < 0)
                return FinancialHealth.Critico;

            return FinancialHealth.Normal;
        }

        private string GenerateAlerts(FamilyBudget budget)
        {
            var alerts = new List<string>();

            // Alerta: Bebé acima do orçamento
            if (budget.BudgetBaby > 0 && budget.ActualBaby > budget.BudgetBaby)
            {
                var excess = ((budget.ActualBaby - budget.BudgetBaby) / budget.BudgetBaby) * 100;
                alerts.Add($"⚠️ Despesas com bebé {excess:F0}% acima do orçamento ({budget.ActualBaby - budget.BudgetBaby:C2})");
            }

            // Alerta: Casa acima do orçamento
            if (budget.BudgetHouse > 0 && budget.ActualHouse > budget.BudgetHouse)
            {
                alerts.Add($"🏠 Despesas com casa acima do previsto");
            }

            // Alerta: Alimentação acima do orçamento
            if (budget.BudgetFood > 0 && budget.ActualFood > budget.BudgetFood)
            {
                alerts.Add($"🛒 Gastos com alimentação acima do orçamento");
            }

            // Alerta geral: gastar mais de 90% do rendimento
            var totalActual = budget.ActualBaby + budget.ActualHouse + budget.ActualFood + 
                            budget.ActualTransport + budget.ActualHealth + budget.ActualLeisure + 
                            budget.ActualOther;

            if (budget.TotalIncome > 0)
            {
                var spentPercentage = (totalActual / budget.TotalIncome) * 100;

                if (spentPercentage > 100)
                {
                    alerts.Add($"🚨 CRÍTICO: Gastos excedem o rendimento em {totalActual - budget.TotalIncome:C2}");
                }
                else if (spentPercentage > 90)
                {
                    alerts.Add($"⚠️ Estás a gastar {spentPercentage:F0}% do rendimento. Cuidado com imprevistos!");
                }
                else if (spentPercentage < 70)
                {
                    var savings = budget.TotalIncome - totalActual;
                    alerts.Add($"✅ Ótimo! Poupaste {savings:C2} este mês ({100 - spentPercentage:F0}% do rendimento)");
                }
            }

            return string.Join("\n", alerts);
        }

        /// <summary>
        /// Prevê despesas do próximo mês baseado em histórico
        /// </summary>
        public async Task<decimal> ForecastNextMonth(string userId, string category)
        {
            var lastThreeMonths = await _context.FamilyBudgets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => new DateTime(b.Year, b.Month, 1))
                .Take(3)
                .ToListAsync();

            if (!lastThreeMonths.Any()) return 0;

            // Média móvel dos últimos 3 meses
            decimal average = category.ToLower() switch
            {
                "baby" or "bebé" => lastThreeMonths.Average(b => b.ActualBaby),
                "house" or "casa" => lastThreeMonths.Average(b => b.ActualHouse),
                "food" or "alimentação" => lastThreeMonths.Average(b => b.ActualFood),
                "transport" or "transporte" => lastThreeMonths.Average(b => b.ActualTransport),
                "health" or "saúde" => lastThreeMonths.Average(b => b.ActualHealth),
                "leisure" or "lazer" => lastThreeMonths.Average(b => b.ActualLeisure),
                _ => 0
            };

            // Adicionar 5% de margem de segurança
            return average * 1.05m;
        }

        /// <summary>
        /// Recomenda um orçamento baseado no rendimento e no estilo de vida
        /// </summary>
        public FamilyBudget RecommendBudget(decimal monthlyIncome, bool hasBaby)
        {
            var budget = new FamilyBudget
            {
                TotalIncome = monthlyIncome
            };

            // Distribuição recomendada (baseada na regra 50/30/20 adaptada para pais)
            if (hasBaby)
            {
                budget.BudgetHouse = monthlyIncome * 0.30m;      // 30% Casa
                budget.BudgetFood = monthlyIncome * 0.15m;       // 15% Alimentação
                budget.BudgetBaby = monthlyIncome * 0.20m;       // 20% Bebé
                budget.BudgetTransport = monthlyIncome * 0.10m;  // 10% Transporte
                budget.BudgetHealth = monthlyIncome * 0.05m;     // 5% Saúde
                budget.BudgetLeisure = monthlyIncome * 0.05m;    // 5% Lazer
                budget.BudgetSavings = monthlyIncome * 0.10m;    // 10% Poupanças
                budget.BudgetOther = monthlyIncome * 0.05m;      // 5% Outros
            }
            else
            {
                budget.BudgetHouse = monthlyIncome * 0.35m;
                budget.BudgetFood = monthlyIncome * 0.15m;
                budget.BudgetBaby = 0;
                budget.BudgetTransport = monthlyIncome * 0.10m;
                budget.BudgetHealth = monthlyIncome * 0.05m;
                budget.BudgetLeisure = monthlyIncome * 0.10m;
                budget.BudgetSavings = monthlyIncome * 0.20m;
                budget.BudgetOther = monthlyIncome * 0.05m;
            }

            return budget;
        }
    }
}
