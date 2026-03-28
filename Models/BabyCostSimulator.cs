using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    /// <summary>
    /// Simulador de Custos do Bebé
    /// Estima custos em diferentes fases: Gravidez, Nascimento, 0-6 meses, 6-12 meses
    /// </summary>
    public class BabyCostSimulator
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Dados de entrada do utilizador
        [Required]
        [Display(Name = "Rendimento Mensal Líquido (€)")]
        [Range(500, 20000)]
        public decimal MonthlyIncome { get; set; }

        [Display(Name = "Estilo de Vida")]
        public LifestyleLevel Lifestyle { get; set; } = LifestyleLevel.Moderado;

        [Display(Name = "Tipo de Parto Previsto")]
        public BirthType ExpectedBirthType { get; set; } = BirthType.PublicoNormal;

        [Display(Name = "Semanas de Gravidez")]
        [Range(0, 42)]
        public int PregnancyWeeks { get; set; }

        [Display(Name = "Data Prevista do Parto")]
        public DateTime? ExpectedDueDate { get; set; }

        [Display(Name = "Tem Apoio Governamental?")]
        public bool HasGovernmentSupport { get; set; } = true;

        [Display(Name = "Vai Amamentar?")]
        public bool WillBreastfeed { get; set; } = true;

        [Display(Name = "Tem Items Oferecidos?")]
        public bool HasDonatedItems { get; set; } = false;

        // Custos estimados calculados
        public decimal EstimatedPregnancyCost { get; set; }
        public decimal EstimatedBirthCost { get; set; }
        public decimal Estimated0to6MonthsCost { get; set; }
        public decimal Estimated6to12MonthsCost { get; set; }
        public decimal EstimatedTotalFirstYear { get; set; }
        public decimal EstimatedMonthlyImpact { get; set; }

        // Tracking
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedDate { get; set; }

        // Alertas e recomendações
        public string? Recommendations { get; set; }
    }

    public enum LifestyleLevel
    {
        [Display(Name = "Económico")]
        Economico = 1,

        [Display(Name = "Moderado")]
        Moderado = 2,

        [Display(Name = "Confortável")]
        Confortavel = 3,

        [Display(Name = "Premium")]
        Premium = 4
    }

    public enum BirthType
    {
        [Display(Name = "Parto Normal - SNS")]
        PublicoNormal = 1,

        [Display(Name = "Cesariana - SNS")]
        PublicoCesariana = 2,

        [Display(Name = "Parto Normal - Privado")]
        PrivadoNormal = 3,

        [Display(Name = "Cesariana - Privado")]
        PrivadoCesariana = 4
    }
}
