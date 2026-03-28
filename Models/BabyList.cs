using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    /// <summary>
    /// Sistema Unificado de Listas do Bebé
    /// Combina funcionalidades de Enxoval (planeamento pessoal) e Lista de Desejos (partilha)
    /// </summary>
    public class BabyList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da lista é obrigatório")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Tipo de lista
        /// </summary>
        public ListType Type { get; set; } = ListType.Enxoval;

        /// <summary>
        /// ID do proprietário principal
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Email do criador (para partilha)
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Data prevista de nascimento
        /// </summary>
        public DateTime? ExpectedDate { get; set; }

        /// <summary>
        /// Nome do bebé (se já escolhido)
        /// </summary>
        [StringLength(100)]
        public string? BabyName { get; set; }

        // ========== FUNCIONALIDADES DE PARTILHA (quando IsShared = true) ==========

        /// <summary>
        /// Se true, ativa sistema de partilha e reservas
        /// </summary>
        public bool IsShared { get; set; } = false;

        /// <summary>
        /// Código único para partilha (gerado quando IsShared = true)
        /// </summary>
        [StringLength(50)]
        public string? ShareCode { get; set; }

        /// <summary>
        /// Se true, qualquer pessoa com o link pode ver
        /// Se false, apenas convidados específicos
        /// </summary>
        public bool IsPublic { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        // Navegação
        public virtual ICollection<BabyListItem> Items { get; set; } = new List<BabyListItem>();
        public virtual ICollection<BabyListManager> Managers { get; set; } = new List<BabyListManager>();
        public virtual ICollection<BabyListShare> Shares { get; set; } = new List<BabyListShare>();
    }

    /// <summary>
    /// Item individual na lista do bebé
    /// </summary>
    public class BabyListItem
    {
        public int Id { get; set; }

        public int BabyListId { get; set; }
        public virtual BabyList BabyList { get; set; } = null!;

        [Required(ErrorMessage = "O nome do item é obrigatório")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        // ========== CATEGORIZAÇÃO ==========

        [Required]
        [Display(Name = "Categoria")]
        public ItemCategory Category { get; set; }

        [Display(Name = "Prioridade")]
        public ItemPriority Priority { get; set; } = ItemPriority.Essencial;

        // ========== PLANEAMENTO FINANCEIRO ==========

        [Display(Name = "Custo Estimado (€)")]
        [Range(0, 10000)]
        public decimal EstimatedCost { get; set; }

        [Display(Name = "Custo Real (€)")]
        public decimal? ActualCost { get; set; }

        // ========== QUANTIDADES ==========

        [Display(Name = "Quantidade")]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Quantidade reservada (quando IsShared = true)
        /// </summary>
        public int ReservedQuantity { get; set; } = 0;

        /// <summary>
        /// Quantidade já adquirida
        /// </summary>
        public int AcquiredQuantity { get; set; } = 0;

        // ========== PLANEAMENTO DE TIMING (Enxoval) ==========

        [Display(Name = "Quando Comprar")]
        public PurchaseTiming? RecommendedTiming { get; set; }

        // ========== STATUS ==========

        [Display(Name = "Já Comprado?")]
        public bool IsPurchased { get; set; } = false;

        [Display(Name = "Data da Compra")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Foi Presente?")]
        public bool IsGift { get; set; } = false;

        // ========== LINKS E MULTIMÉDIA ==========

        [StringLength(500)]
        [Display(Name = "Link do Produto")]
        public string? ProductUrl { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(1000)]
        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;

        // Navegação
        public virtual ICollection<BabyListReservation> Reservations { get; set; } = new List<BabyListReservation>();

        /// <summary>
        /// Quantidade disponível para reservar/comprar
        /// </summary>
        public int AvailableQuantity => Quantity - ReservedQuantity - AcquiredQuantity;

        /// <summary>
        /// Verifica se item está completamente reservado/adquirido
        /// </summary>
        public bool IsFullyReserved => AvailableQuantity <= 0;
    }

    /// <summary>
    /// Gestor/Co-gestor da lista (para listas partilhadas)
    /// </summary>
    public class BabyListManager
    {
        public int Id { get; set; }

        public int BabyListId { get; set; }
        public virtual BabyList BabyList { get; set; } = null!;

        [Required]
        public string ManagerEmail { get; set; } = string.Empty;

        [StringLength(100)]
        public string ManagerName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Role { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public bool CanManageManagers { get; set; } = true;
    }

    /// <summary>
    /// Partilha da lista com outras pessoas
    /// </summary>
    public class BabyListShare
    {
        public int Id { get; set; }

        public int BabyListId { get; set; }
        public virtual BabyList BabyList { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string SharedWithEmail { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SharedWithName { get; set; }

        [StringLength(500)]
        public string? InviteMessage { get; set; }

        public DateTime SharedDate { get; set; } = DateTime.UtcNow;
        public string SharedBy { get; set; } = string.Empty;

        public bool IsAccepted { get; set; } = false;
        public DateTime? AcceptedDate { get; set; }

        [StringLength(20)]
        public string Permission { get; set; } = "Reserve";
    }

    /// <summary>
    /// Reserva de um item por um convidado
    /// </summary>
    public class BabyListReservation
    {
        public int Id { get; set; }

        public int BabyListItemId { get; set; }
        public virtual BabyListItem BabyListItem { get; set; } = null!;

        [Required]
        public string ReservedBy { get; set; } = string.Empty;

        [StringLength(100)]
        public string ReservedByName { get; set; } = string.Empty;

        public int Quantity { get; set; } = 1;

        public DateTime ReservedDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Message { get; set; }

        /// <summary>
        /// Email para os pais entrarem em contacto
        /// </summary>
        [EmailAddress]
        public string? ContactEmail { get; set; }
    }

    // ========== ENUMS ==========

    public enum ListType
    {
        [Display(Name = "🛒 Enxoval (Planeamento Pessoal)")]
        Enxoval = 1,

        [Display(Name = "🎁 Lista de Presentes")]
        Presentes = 2,

        [Display(Name = "📋 Lista Geral")]
        Geral = 3
    }

    public enum ItemCategory
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

        [Display(Name = "Acessórios")]
        Acessorios = 10,

        [Display(Name = "Outros")]
        Outros = 11
    }

    public enum ItemPriority
    {
        [Display(Name = "✅ Essencial")]
        Essencial = 1,

        [Display(Name = "⭐ Recomendado")]
        Recomendado = 2,

        [Display(Name = "💡 Opcional")]
        Opcional = 3
    }

    // Nota: PurchaseTiming já está definido em BabyShoppingItem.cs
}
