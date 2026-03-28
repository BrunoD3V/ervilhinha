using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    /// <summary>
    /// Orçamento Familiar Inteligente
    /// Sistema de previsão e controlo de despesas familiares e do bebé
    /// </summary>
    public class FamilyBudget
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Mês")]
        public int Month { get; set; }

        [Required]
        [Display(Name = "Ano")]
        public int Year { get; set; }

        // Rendimentos
        [Display(Name = "Rendimento Total")]
        public decimal TotalIncome { get; set; }

        // Limites de orçamento por categoria
        [Display(Name = "Limite - Bebé")]
        public decimal BudgetBaby { get; set; }

        [Display(Name = "Limite - Casa")]
        public decimal BudgetHouse { get; set; }

        [Display(Name = "Limite - Alimentação")]
        public decimal BudgetFood { get; set; }

        [Display(Name = "Limite - Transporte")]
        public decimal BudgetTransport { get; set; }

        [Display(Name = "Limite - Saúde")]
        public decimal BudgetHealth { get; set; }

        [Display(Name = "Limite - Lazer")]
        public decimal BudgetLeisure { get; set; }

        [Display(Name = "Limite - Poupanças")]
        public decimal BudgetSavings { get; set; }

        [Display(Name = "Limite - Outros")]
        public decimal BudgetOther { get; set; }

        // Gastos reais (calculados automaticamente das despesas)
        public decimal ActualBaby { get; set; }
        public decimal ActualHouse { get; set; }
        public decimal ActualFood { get; set; }
        public decimal ActualTransport { get; set; }
        public decimal ActualHealth { get; set; }
        public decimal ActualLeisure { get; set; }
        public decimal ActualOther { get; set; }

        // Previsões para próximo mês
        public decimal ForecastBaby { get; set; }
        public decimal ForecastTotal { get; set; }

        // Estado de saúde financeira
        public FinancialHealth HealthStatus { get; set; } = FinancialHealth.Normal;

        [StringLength(2000)]
        public string? AlertMessages { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedDate { get; set; }
    }

    public enum FinancialHealth
    {
        [Display(Name = "Excelente")]
        Excelente = 1,

        [Display(Name = "Bom")]
        Bom = 2,

        [Display(Name = "Normal")]
        Normal = 3,

        [Display(Name = "Atenção")]
        Atencao = 4,

        [Display(Name = "Crítico")]
        Critico = 5
    }
}
