using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    /// <summary>
    /// Sistema de Alertas e Recomendações Inteligentes
    /// </summary>
    public class SmartAlert
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Título")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        [Display(Name = "Mensagem")]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tipo de Alerta")]
        public AlertType Type { get; set; }

        [Display(Name = "Categoria")]
        public AlertCategory Category { get; set; }

        [Display(Name = "Prioridade")]
        public AlertPriority Priority { get; set; } = AlertPriority.Media;

        [Display(Name = "Valor Associado (€)")]
        public decimal? AssociatedAmount { get; set; }

        [Display(Name = "Link de Ação")]
        [StringLength(500)]
        public string? ActionLink { get; set; }

        [Display(Name = "Texto do Botão")]
        [StringLength(100)]
        public string? ActionButtonText { get; set; }

        [Display(Name = "Lido")]
        public bool IsRead { get; set; } = false;

        [Display(Name = "Data de Leitura")]
        public DateTime? ReadDate { get; set; }

        [Display(Name = "Dispensado")]
        public bool IsDismissed { get; set; } = false;

        [Display(Name = "Data de Criação")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Expira em")]
        public DateTime? ExpiryDate { get; set; }

        // Para alertas recorrentes
        [Display(Name = "É Recorrente")]
        public bool IsRecurring { get; set; } = false;

        [Display(Name = "Recorrência")]
        public RecurrenceType? RecurrencePattern { get; set; }
    }

    public enum AlertType
    {
        [Display(Name = "💰 Financeiro")]
        Financeiro = 1,

        [Display(Name = "📋 Tarefa")]
        Tarefa = 2,

        [Display(Name = "💡 Recomendação")]
        Recomendacao = 3,

        [Display(Name = "⚠️ Aviso")]
        Aviso = 4,

        [Display(Name = "✅ Sucesso")]
        Sucesso = 5,

        [Display(Name = "📊 Insight")]
        Insight = 6
    }

    public enum AlertCategory
    {
        [Display(Name = "Orçamento")]
        Orcamento = 1,

        [Display(Name = "Despesas")]
        Despesas = 2,

        [Display(Name = "Compras do Bebé")]
        ComprasBebe = 3,

        [Display(Name = "Timeline")]
        Timeline = 4,

        [Display(Name = "Poupanças")]
        Poupancas = 5,

        [Display(Name = "Saúde Financeira")]
        SaudeFinanceira = 6,

        [Display(Name = "Geral")]
        Geral = 7
    }

    public enum AlertPriority
    {
        [Display(Name = "Alta")]
        Alta = 1,

        [Display(Name = "Média")]
        Media = 2,

        [Display(Name = "Baixa")]
        Baixa = 3
    }

    public enum RecurrenceType
    {
        [Display(Name = "Diário")]
        Diario = 1,

        [Display(Name = "Semanal")]
        Semanal = 2,

        [Display(Name = "Mensal")]
        Mensal = 3
    }
}
