using Ervilhinha.Models;
using Ervilhinha.Data;
using Microsoft.EntityFrameworkCore;

namespace Ervilhinha.Services
{
    /// <summary>
    /// Serviço de Geração e Gestão de Alertas Inteligentes
    /// </summary>
    public class SmartAlertService
    {
        private readonly ApplicationDbContext _context;

        public SmartAlertService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gera alertas baseados no orçamento do utilizador
        /// </summary>
        public async Task GenerateBudgetAlerts(string userId, FamilyBudget budget)
        {
            // Alerta: Excesso em categoria específica
            if (budget.ActualBaby > budget.BudgetBaby && budget.BudgetBaby > 0)
            {
                var excess = budget.ActualBaby - budget.BudgetBaby;
                var percentage = (excess / budget.BudgetBaby) * 100;

                await CreateAlert(new SmartAlert
                {
                    UserId = userId,
                    Title = "Orçamento do Bebé Excedido",
                    Message = $"As despesas com o bebé este mês excederam o orçamento em {excess:C2} ({percentage:F0}%). Considera ajustar os gastos nos próximos dias.",
                    Type = AlertType.Financeiro,
                    Category = AlertCategory.Orcamento,
                    Priority = percentage > 50 ? AlertPriority.Alta : AlertPriority.Media,
                    AssociatedAmount = excess,
                    ActionLink = "/FamilyBudget/Index",
                    ActionButtonText = "Ver Orçamento",
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                });
            }

            // Alerta: Saúde financeira crítica
            if (budget.HealthStatus == FinancialHealth.Critico)
            {
                await CreateAlert(new SmartAlert
                {
                    UserId = userId,
                    Title = "🚨 Situação Financeira Crítica",
                    Message = "Os teus gastos excedem o rendimento este mês. É urgente rever as despesas e cortar custos não essenciais.",
                    Type = AlertType.Aviso,
                    Category = AlertCategory.SaudeFinanceira,
                    Priority = AlertPriority.Alta,
                    ActionLink = "/FamilyBudget/Analysis",
                    ActionButtonText = "Analisar Situação"
                });
            }

            // Alerta: Poupanças abaixo do ideal
            var totalExpenses = budget.ActualBaby + budget.ActualHouse + budget.ActualFood + 
                               budget.ActualTransport + budget.ActualHealth + budget.ActualLeisure + budget.ActualOther;
            var savings = budget.TotalIncome - totalExpenses;
            var savingsPercentage = budget.TotalIncome > 0 ? (savings / budget.TotalIncome) * 100 : 0;

            if (savingsPercentage < 10 && savingsPercentage >= 0)
            {
                await CreateAlert(new SmartAlert
                {
                    UserId = userId,
                    Title = "Poupanças Abaixo do Ideal",
                    Message = $"Este mês conseguiste poupar apenas {savings:C2} ({savingsPercentage:F1}%). O ideal seria poupar pelo menos 10-15% do rendimento.",
                    Type = AlertType.Recomendacao,
                    Category = AlertCategory.Poupancas,
                    Priority = AlertPriority.Media,
                    AssociatedAmount = savings
                });
            }

            // Alerta positivo: Poupou bem
            if (savingsPercentage >= 20)
            {
                await CreateAlert(new SmartAlert
                {
                    UserId = userId,
                    Title = "🎉 Excelente Gestão Financeira!",
                    Message = $"Parabéns! Conseguiste poupar {savings:C2} este mês ({savingsPercentage:F0}% do rendimento). Continua assim!",
                    Type = AlertType.Sucesso,
                    Category = AlertCategory.Poupancas,
                    Priority = AlertPriority.Baixa,
                    AssociatedAmount = savings,
                    ExpiryDate = DateTime.UtcNow.AddDays(30)
                });
            }
        }

        /// <summary>
        /// Gera alertas baseados na timeline do bebé
        /// </summary>
        public async Task GenerateTimelineAlerts(string userId)
        {
            var upcomingEvents = await _context.BabyTimelines
                .Where(t => t.UserId == userId && !t.IsCompleted && t.EventDate.HasValue)
                .Where(t => t.EventDate.Value >= DateTime.Today && t.EventDate.Value <= DateTime.Today.AddDays(14))
                .OrderBy(t => t.EventDate)
                .ToListAsync();

            foreach (var evt in upcomingEvents)
            {
                if (!evt.EventDate.HasValue) continue;

                var daysUntil = (evt.EventDate.Value - DateTime.Today).Days;
                var urgency = daysUntil <= 3 ? AlertPriority.Alta : AlertPriority.Media;

                await CreateAlert(new SmartAlert
                {
                    UserId = userId,
                    Title = $"Próximo: {evt.Title}",
                    Message = $"Daqui a {daysUntil} dia(s): {evt.Title}. {evt.Recommendation ?? ""}",
                    Type = AlertType.Tarefa,
                    Category = AlertCategory.Timeline,
                    Priority = urgency,
                    AssociatedAmount = evt.EstimatedCost,
                    ActionLink = $"/BabyTimeline/Details/{evt.Id}",
                    ActionButtonText = "Ver Detalhes",
                    ExpiryDate = evt.EventDate
                });
            }
        }

        /// <summary>
        /// Gera alertas baseados em compras pendentes do enxoval
        /// </summary>
        public async Task GenerateShoppingAlerts(string userId, DateTime? dueDate)
        {
            if (!dueDate.HasValue) return;

            var essentialItems = await _context.BabyShoppingItems
                .Where(s => s.UserId == userId && !s.IsPurchased)
                .Where(s => s.Priority == ShoppingPriority.Essencial)
                .ToListAsync();

            if (essentialItems.Any())
            {
                var totalCost = essentialItems.Sum(i => i.EstimatedCost * i.Quantity);
                var daysUntilBirth = (dueDate.Value - DateTime.Today).Days;

                if (daysUntilBirth <= 60 && daysUntilBirth > 0) // Últimos 2 meses
                {
                    await CreateAlert(new SmartAlert
                    {
                        UserId = userId,
                        Title = "Itens Essenciais Pendentes",
                        Message = $"Ainda tens {essentialItems.Count} itens essenciais por comprar (total: {totalCost:C2}). Faltam {daysUntilBirth} dias para o nascimento!",
                        Type = AlertType.Aviso,
                        Category = AlertCategory.ComprasBebe,
                        Priority = daysUntilBirth <= 30 ? AlertPriority.Alta : AlertPriority.Media,
                        AssociatedAmount = totalCost,
                        ActionLink = "/BabyShopping/Index",
                        ActionButtonText = "Ver Lista de Compras"
                    });
                }
            }
        }

        /// <summary>
        /// Gera insights baseados em padrões de despesas
        /// </summary>
        public async Task GenerateInsights(string userId)
        {
            var lastThreeMonths = await _context.FamilyBudgets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => new DateTime(b.Year, b.Month, 1))
                .Take(3)
                .ToListAsync();

            if (lastThreeMonths.Count >= 3)
            {
                // Insight: Tendência crescente de gastos com bebé
                var babyExpenses = lastThreeMonths.Select(b => b.ActualBaby).ToList();
                if (babyExpenses[0] > babyExpenses[1] && babyExpenses[1] > babyExpenses[2])
                {
                    var increase = babyExpenses[0] - babyExpenses[2];
                    var percentage = (increase / babyExpenses[2]) * 100;

                    await CreateAlert(new SmartAlert
                    {
                        UserId = userId,
                        Title = "📊 Tendência de Gastos com Bebé",
                        Message = $"As despesas com o bebé aumentaram {percentage:F0}% nos últimos 3 meses. Isto é normal à medida que o bebé cresce, mas fica atento ao orçamento.",
                        Type = AlertType.Insight,
                        Category = AlertCategory.Despesas,
                        Priority = AlertPriority.Baixa,
                        AssociatedAmount = increase,
                        ExpiryDate = DateTime.UtcNow.AddDays(15)
                    });
                }

                // Insight: Mês mais económico
                var minExpense = lastThreeMonths.Min(b => 
                    b.ActualBaby + b.ActualHouse + b.ActualFood + b.ActualTransport + 
                    b.ActualHealth + b.ActualLeisure + b.ActualOther);
                
                var bestMonth = lastThreeMonths.First(b => 
                    (b.ActualBaby + b.ActualHouse + b.ActualFood + b.ActualTransport + 
                     b.ActualHealth + b.ActualLeisure + b.ActualOther) == minExpense);

                await CreateAlert(new SmartAlert
                {
                    UserId = userId,
                    Title = "💡 Insight: Mês Mais Económico",
                    Message = $"O teu melhor mês foi {new DateTime(bestMonth.Year, bestMonth.Month, 1):MMMM yyyy} com gastos de {minExpense:C2}. Tenta repetir esse padrão!",
                    Type = AlertType.Insight,
                    Category = AlertCategory.SaudeFinanceira,
                    Priority = AlertPriority.Baixa,
                    ExpiryDate = DateTime.UtcNow.AddDays(30)
                });
            }
        }

        /// <summary>
        /// Cria um novo alerta (evita duplicados)
        /// </summary>
        private async Task CreateAlert(SmartAlert alert)
        {
            // Verificar se já existe um alerta similar recente (últimas 24h)
            var existingAlert = await _context.SmartAlerts
                .Where(a => a.UserId == alert.UserId)
                .Where(a => a.Title == alert.Title)
                .Where(a => a.CreatedDate >= DateTime.UtcNow.AddHours(-24))
                .Where(a => !a.IsDismissed)
                .FirstOrDefaultAsync();

            if (existingAlert == null)
            {
                _context.SmartAlerts.Add(alert);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Marca um alerta como lido
        /// </summary>
        public async Task MarkAsRead(int alertId)
        {
            var alert = await _context.SmartAlerts.FindAsync(alertId);
            if (alert != null)
            {
                alert.IsRead = true;
                alert.ReadDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Dispensa um alerta
        /// </summary>
        public async Task DismissAlert(int alertId)
        {
            var alert = await _context.SmartAlerts.FindAsync(alertId);
            if (alert != null)
            {
                alert.IsDismissed = true;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Obtém alertas ativos do utilizador
        /// </summary>
        public async Task<List<SmartAlert>> GetActiveAlerts(string userId, int count = 10)
        {
            return await _context.SmartAlerts
                .Where(a => a.UserId == userId)
                .Where(a => !a.IsDismissed)
                .Where(a => !a.ExpiryDate.HasValue || a.ExpiryDate.Value >= DateTime.UtcNow)
                .OrderByDescending(a => a.Priority)
                .ThenByDescending(a => a.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Limpa alertas expirados
        /// </summary>
        public async Task CleanupExpiredAlerts()
        {
            var expiredAlerts = await _context.SmartAlerts
                .Where(a => a.ExpiryDate.HasValue && a.ExpiryDate.Value < DateTime.UtcNow)
                .ToListAsync();

            _context.SmartAlerts.RemoveRange(expiredAlerts);
            await _context.SaveChangesAsync();
        }
    }
}
