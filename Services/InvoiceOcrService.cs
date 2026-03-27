using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace Ervilhinha.Services
{
    public interface IInvoiceOcrService
    {
        Task<InvoiceOcrResult> ProcessInvoiceAsync(Stream imageStream);
    }

    public class InvoiceOcrResult
    {
        public bool Success { get; set; }
        public string? VendorName { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? RawText { get; set; }
        public string? ErrorMessage { get; set; }
        public List<LineItem> LineItems { get; set; } = new();
    }

    public class LineItem
    {
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
    }

    public class AzureInvoiceOcrService : IInvoiceOcrService
    {
        private readonly string? _endpoint;
        private readonly string? _apiKey;
        private readonly ILogger<AzureInvoiceOcrService> _logger;

        public AzureInvoiceOcrService(IConfiguration configuration, ILogger<AzureInvoiceOcrService> logger)
        {
            _endpoint = configuration["AzureFormRecognizer:Endpoint"];
            _apiKey = configuration["AzureFormRecognizer:ApiKey"];
            _logger = logger;
        }

        public async Task<InvoiceOcrResult> ProcessInvoiceAsync(Stream imageStream)
        {
            var result = new InvoiceOcrResult();

            try
            {
                // Check if Azure Form Recognizer is configured
                if (string.IsNullOrEmpty(_endpoint) || string.IsNullOrEmpty(_apiKey))
                {
                    _logger.LogWarning("Azure Form Recognizer not configured. Using mock data.");
                    return GetMockResult();
                }

                var credential = new AzureKeyCredential(_apiKey);
                var client = new DocumentAnalysisClient(new Uri(_endpoint), credential);

                var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", imageStream);
                var invoiceResult = operation.Value;

                if (invoiceResult.Documents.Count > 0)
                {
                    var invoice = invoiceResult.Documents[0];

                    result.Success = true;
                    result.VendorName = GetFieldValue(invoice, "VendorName");
                    
                    if (invoice.Fields.TryGetValue("InvoiceTotal", out var totalField) && totalField.FieldType == DocumentFieldType.Currency)
                    {
                        var currencyValue = totalField.Value.AsCurrency();
                        result.TotalAmount = (decimal?)currencyValue.Amount;
                    }

                    if (invoice.Fields.TryGetValue("InvoiceDate", out var dateField) && dateField.FieldType == DocumentFieldType.Date)
                    {
                        var dateValue = dateField.Value.AsDate();
                        result.InvoiceDate = dateValue.DateTime;
                    }

                    // Extract line items
                    if (invoice.Fields.TryGetValue("Items", out var itemsField) && itemsField.FieldType == DocumentFieldType.List)
                    {
                        foreach (var itemField in itemsField.Value.AsList())
                        {
                            if (itemField.FieldType == DocumentFieldType.Dictionary)
                            {
                                var item = itemField.Value.AsDictionary();
                                var lineItem = new LineItem();

                                if (item.TryGetValue("Description", out var desc))
                                {
                                    lineItem.Description = desc.Content;
                                }

                                if (item.TryGetValue("Amount", out var amount) && amount.FieldType == DocumentFieldType.Currency)
                                {
                                    var amountValue = amount.Value.AsCurrency();
                                    lineItem.Amount = (decimal?)amountValue.Amount;
                                }

                                result.LineItems.Add(lineItem);
                            }
                        }
                    }

                    result.RawText = invoiceResult.Content;
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "No invoice data could be extracted from the image.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invoice with Azure Form Recognizer");
                result.Success = false;
                result.ErrorMessage = $"OCR Error: {ex.Message}";
            }

            return result;
        }

        private string? GetFieldValue(AnalyzedDocument document, string fieldName)
        {
            if (document.Fields.TryGetValue(fieldName, out var field))
            {
                return field.Content;
            }
            return null;
        }

        private InvoiceOcrResult GetMockResult()
        {
            // Return mock data for development when Azure is not configured
            return new InvoiceOcrResult
            {
                Success = true,
                VendorName = "Sample Store (Mock Data)",
                TotalAmount = 45.99m,
                InvoiceDate = DateTime.Today.AddDays(-2),
                RawText = "This is mock OCR data. Configure Azure Form Recognizer for real invoice processing.",
                LineItems = new List<LineItem>
                {
                    new LineItem { Description = "Sample Item 1", Amount = 25.99m },
                    new LineItem { Description = "Sample Item 2", Amount = 20.00m }
                }
            };
        }
    }
}
