using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        public int ExpenseCategoryId { get; set; }

        public ExpenseCategory? ExpenseCategory { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }
    }
}
