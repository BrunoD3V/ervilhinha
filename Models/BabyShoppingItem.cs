using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    /// <summary>
    /// Item do Planeador de Compras para o Bebé (Enxoval Inteligente)
    /// </summary>
    public class BabyShoppingItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Nome do Item")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Categoria")]
        public ShoppingCategory Category { get; set; }

        [Display(Name = "Prioridade")]
        public ShoppingPriority Priority { get; set; } = ShoppingPriority.Essencial;

        [Display(Name = "Custo Estimado (€)")]
        [Range(0, 10000)]
        public decimal EstimatedCost { get; set; }

        [Display(Name = "Quando Comprar")]
        public PurchaseTiming RecommendedTiming { get; set; } = PurchaseTiming.Mes7;

        [Display(Name = "Quantidade")]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Já Comprado?")]
        public bool IsPurchased { get; set; } = false;

        [Display(Name = "Data da Compra")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Custo Real (€)")]
        public decimal? ActualCost { get; set; }

        [Display(Name = "Oferta/Doação?")]
        public bool IsGift { get; set; } = false;

        [StringLength(1000)]
        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        [StringLength(500)]
        [Display(Name = "Link da Loja")]
        public string? StoreLink { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }

    public enum ShoppingCategory
    {
        [Display(Name = "Quarto do Bebé")]
        QuartoBebe = 1,

        [Display(Name = "Roupa 0-3M")]
        Roupa0a3 = 2,

        [Display(Name = "Roupa 3-6M")]
        Roupa3a6 = 3,

        [Display(Name = "Roupa 6-12M")]
        Roupa6a12 = 4,

        [Display(Name = "Higiene e Banho")]
        HigieneBanho = 5,

        [Display(Name = "Alimentação")]
        Alimentacao = 6,

        [Display(Name = "Passeio (Carrinho, Cadeira Auto)")]
        Passeio = 7,

        [Display(Name = "Brinquedos")]
        Brinquedos = 8,

        [Display(Name = "Saúde e Segurança")]
        SaudeSeguranca = 9,

        [Display(Name = "Outros")]
        Outros = 10
    }

    public enum ShoppingPriority
    {
        [Display(Name = "✅ Essencial")]
        Essencial = 1,

        [Display(Name = "⭐ Recomendado")]
        Recomendado = 2,

        [Display(Name = "💡 Opcional")]
        Opcional = 3,

        [Display(Name = "❌ Evitar")]
        Evitar = 4
    }

    public enum PurchaseTiming
    {
        [Display(Name = "Gravidez (até 6 meses)")]
        Ate6Meses = 1,

        [Display(Name = "7º Mês")]
        Mes7 = 2,

        [Display(Name = "8º Mês")]
        Mes8 = 3,

        [Display(Name = "Antes do Nascimento")]
        AntesNascimento = 4,

        [Display(Name = "Após Nascimento")]
        AposNascimento = 5,

        [Display(Name = "Com 3 Meses")]
        Com3Meses = 6,

        [Display(Name = "Com 6 Meses")]
        Com6Meses = 7,

        [Display(Name = "Com 9 Meses")]
        Com9Meses = 8,

        [Display(Name = "Quando Necessário")]
        QuandoNecessario = 9
    }
}
