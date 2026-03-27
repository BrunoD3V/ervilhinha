using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    public class BabyItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public int Priority { get; set; }

        public bool IsPurchased { get; set; }

        public decimal? EstimatedCost { get; set; }

        public decimal? ActualCost { get; set; }

        public string? PurchasedBy { get; set; }

        public DateTime? PurchasedDate { get; set; }

        public string? Notes { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }
    }
}
