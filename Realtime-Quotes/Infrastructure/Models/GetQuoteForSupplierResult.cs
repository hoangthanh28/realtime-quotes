namespace RealtimeQuotes.Infrastructure.Models
{
    public class GetQuoteForSupplierResult
    {
        public string City { get; set; }

        public string Supplier { get; set; }

        public string TaskId { get; set; }

        public double Quote { get; set; }

        public double ResponseTime { get; set; }
    }
}
