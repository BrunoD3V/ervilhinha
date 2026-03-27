namespace Ervilhinha.Models
{
    public class ReportsViewModel
    {
        public decimal GrandTotal { get; set; }
        public List<CategoryTotal> CategoryTotals { get; set; } = new();
        public List<MonthlyTotal> MonthlyTotals { get; set; } = new();
    }

    public class CategoryTotal
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Count { get; set; }
    }

    public class MonthlyTotal
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Total { get; set; }
    }
}
