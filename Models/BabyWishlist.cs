using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    /// <summary>
    /// Lista de Desejos para o Bebé
    /// Permite aos pais criar uma lista partilhável de items desejados
    /// </summary>
    public class BabyWishlist
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da lista é obrigatório")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Data prevista de nascimento do bebé
        /// </summary>
        public DateTime? ExpectedDate { get; set; }

        /// <summary>
        /// Nome do bebé (se já escolhido)
        /// </summary>
        [StringLength(100)]
        public string? BabyName { get; set; }

        /// <summary>
        /// Código único para partilha (ex: "ABC123XYZ")
        /// </summary>
        [StringLength(50)]
        public string ShareCode { get; set; } = string.Empty;

        /// <summary>
        /// Se true, qualquer pessoa com o link pode ver
        /// Se false, apenas convidados específicos
        /// </summary>
        public bool IsPublic { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = string.Empty;

        // Navegação
        public virtual ICollection<WishlistManager> Managers { get; set; } = new List<WishlistManager>();
        public virtual ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
        public virtual ICollection<WishlistShare> Shares { get; set; } = new List<WishlistShare>();
    }

    /// <summary>
    /// Gestores da lista (normalmente pai e mãe)
    /// </summary>
    public class WishlistManager
    {
        public int Id { get; set; }

        public int WishlistId { get; set; }
        public virtual BabyWishlist Wishlist { get; set; } = null!;

        /// <summary>
        /// Email/ID do gestor
        /// </summary>
        [Required]
        public string ManagerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Nome do gestor
        /// </summary>
        [StringLength(100)]
        public string ManagerName { get; set; } = string.Empty;

        /// <summary>
        /// Papel: "Pai", "Mãe", "Gestor"
        /// </summary>
        [StringLength(50)]
        public string? Role { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Se true, pode adicionar outros gestores
        /// </summary>
        public bool CanManageManagers { get; set; } = true;
    }

    /// <summary>
    /// Item individual na lista de desejos
    /// </summary>
    public class WishlistItem
    {
        public int Id { get; set; }

        public int WishlistId { get; set; }
        public virtual BabyWishlist Wishlist { get; set; } = null!;

        [Required(ErrorMessage = "O nome do item é obrigatório")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Categoria: "Roupa", "Brinquedos", "Quarto", "Higiene", etc.
        /// </summary>
        [StringLength(50)]
        public string? Category { get; set; }

        /// <summary>
        /// Prioridade: 1 (Alta), 2 (Média), 3 (Baixa)
        /// </summary>
        public int Priority { get; set; } = 2;

        /// <summary>
        /// Preço estimado
        /// </summary>
        public decimal? EstimatedPrice { get; set; }

        /// <summary>
        /// Quantidade desejada
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Quantidade já reservada
        /// </summary>
        public int ReservedQuantity { get; set; } = 0;

        /// <summary>
        /// Quantidade já adquirida (marcada pelos pais)
        /// </summary>
        public int AcquiredQuantity { get; set; } = 0;

        /// <summary>
        /// Link para produto online (opcional)
        /// </summary>
        [StringLength(500)]
        public string? ProductUrl { get; set; }

        /// <summary>
        /// URL da imagem do produto
        /// </summary>
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Notas adicionais
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = string.Empty;

        // Navegação
        public virtual ICollection<WishlistReservation> Reservations { get; set; } = new List<WishlistReservation>();

        /// <summary>
        /// Calcula quantidade disponível
        /// </summary>
        public int AvailableQuantity => Quantity - ReservedQuantity - AcquiredQuantity;

        /// <summary>
        /// Verifica se item está completamente reservado/adquirido
        /// </summary>
        public bool IsFullyReserved => AvailableQuantity <= 0;
    }

    /// <summary>
    /// Partilha da lista com outras pessoas
    /// </summary>
    public class WishlistShare
    {
        public int Id { get; set; }

        public int WishlistId { get; set; }
        public virtual BabyWishlist Wishlist { get; set; } = null!;

        /// <summary>
        /// Email da pessoa convidada
        /// </summary>
        [Required]
        [EmailAddress]
        public string SharedWithEmail { get; set; } = string.Empty;

        /// <summary>
        /// Nome da pessoa convidada
        /// </summary>
        [StringLength(100)]
        public string? SharedWithName { get; set; }

        /// <summary>
        /// Mensagem personalizada no convite
        /// </summary>
        [StringLength(500)]
        public string? InviteMessage { get; set; }

        public DateTime SharedDate { get; set; } = DateTime.UtcNow;

        public string SharedBy { get; set; } = string.Empty;

        /// <summary>
        /// Se true, a pessoa aceitou o convite
        /// </summary>
        public bool IsAccepted { get; set; } = false;

        public DateTime? AcceptedDate { get; set; }

        /// <summary>
        /// Permissões: "View" (apenas ver), "Reserve" (pode reservar)
        /// </summary>
        [StringLength(20)]
        public string Permission { get; set; } = "Reserve";
    }

    /// <summary>
    /// Reserva de um item por um convidado
    /// </summary>
    public class WishlistReservation
    {
        public int Id { get; set; }

        public int WishlistItemId { get; set; }
        public virtual WishlistItem WishlistItem { get; set; } = null!;

        /// <summary>
        /// Email de quem reservou
        /// </summary>
        [Required]
        public string ReservedBy { get; set; } = string.Empty;

        /// <summary>
        /// Nome de quem reservou
        /// </summary>
        [StringLength(100)]
        public string ReservedByName { get; set; } = string.Empty;

        /// <summary>
        /// Quantidade reservada
        /// </summary>
        public int Quantity { get; set; } = 1;

        public DateTime ReservedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Mensagem opcional para os pais
        /// </summary>
        [StringLength(500)]
        public string? Message { get; set; }

        /// <summary>
        /// Se true, foi entregue/confirmado
        /// </summary>
        public bool IsDelivered { get; set; } = false;

        public DateTime? DeliveredDate { get; set; }
    }
}
