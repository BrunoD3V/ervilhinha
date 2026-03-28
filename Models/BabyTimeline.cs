using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    /// <summary>
    /// Linha do Tempo Inteligente do Bebé
    /// Da gravidez ao primeiro ano
    /// </summary>
    public class BabyTimeline
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Evento/Tarefa")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        public TimelineEventType Type { get; set; }

        [Display(Name = "Fase")]
        public BabyPhase Phase { get; set; }

        [Display(Name = "Data/Prazo")]
        public DateTime? EventDate { get; set; }

        [Display(Name = "Semana de Gravidez")]
        [Range(0, 42)]
        public int? PregnancyWeek { get; set; }

        [Display(Name = "Mês de Vida do Bebé")]
        [Range(0, 12)]
        public int? BabyMonthAge { get; set; }

        [Display(Name = "Custo Associado (€)")]
        public decimal? EstimatedCost { get; set; }

        [Display(Name = "Prioridade")]
        public TimelinePriority Priority { get; set; } = TimelinePriority.Media;

        [Display(Name = "Concluído")]
        public bool IsCompleted { get; set; } = false;

        [Display(Name = "Data de Conclusão")]
        public DateTime? CompletedDate { get; set; }

        [Display(Name = "Custo Real (€)")]
        public decimal? ActualCost { get; set; }

        [StringLength(1000)]
        [Display(Name = "Recomendação")]
        public string? Recommendation { get; set; }

        [Display(Name = "Lembrete Ativo")]
        public bool HasReminder { get; set; } = false;

        [Display(Name = "Lembrar X dias antes")]
        public int? ReminderDaysBefore { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }

    public enum TimelineEventType
    {
        [Display(Name = "🏥 Consulta Médica")]
        ConsultaMedica = 1,

        [Display(Name = "💉 Exame/Vacina")]
        ExameVacina = 2,

        [Display(Name = "🛒 Compra Importante")]
        CompraImportante = 3,

        [Display(Name = "🏠 Preparação Casa")]
        PreparacaoCasa = 4,

        [Display(Name = "📋 Tarefa Administrativa")]
        TarefaAdministrativa = 5,

        [Display(Name = "🎓 Curso/Workshop")]
        CursoWorkshop = 6,

        [Display(Name = "👶 Marco do Bebé")]
        MarcoBebe = 7,

        [Display(Name = "💰 Despesa Planeada")]
        DespesaPlaneada = 8,

        [Display(Name = "📅 Outro Evento")]
        OutroEvento = 9
    }

    public enum BabyPhase
    {
        [Display(Name = "1º Trimestre")]
        Trimestre1 = 1,

        [Display(Name = "2º Trimestre")]
        Trimestre2 = 2,

        [Display(Name = "3º Trimestre")]
        Trimestre3 = 3,

        [Display(Name = "Nascimento")]
        Nascimento = 4,

        [Display(Name = "0-3 Meses")]
        Meses0a3 = 5,

        [Display(Name = "3-6 Meses")]
        Meses3a6 = 6,

        [Display(Name = "6-9 Meses")]
        Meses6a9 = 7,

        [Display(Name = "9-12 Meses")]
        Meses9a12 = 8
    }

    public enum TimelinePriority
    {
        [Display(Name = "🔴 Urgente")]
        Urgente = 1,

        [Display(Name = "🟠 Alta")]
        Alta = 2,

        [Display(Name = "🟡 Média")]
        Media = 3,

        [Display(Name = "🟢 Baixa")]
        Baixa = 4
    }
}
