using System.ComponentModel.DataAnnotations;

namespace Ervilhinha.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        public string UploadedBy { get; set; } = string.Empty;

        public bool IsProcessed { get; set; } = false;

        public bool HasErrors { get; set; } = false;

        public string? ErrorMessage { get; set; }

        public decimal? TotalAmount { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string? VendorName { get; set; }

        public string? RawOcrText { get; set; }

        public bool IsReviewed { get; set; } = false;

        public string? ReviewedBy { get; set; }

        public DateTime? ReviewedDate { get; set; }

        public int? ExpenseId { get; set; }

        public Expense? Expense { get; set; }
    }
}
