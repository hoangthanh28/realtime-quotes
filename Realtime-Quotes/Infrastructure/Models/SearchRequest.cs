namespace RealtimeQuotes.Infrastructure.Models
{
    public class SearchRequest
{
        public string Currency { get; set; }
        
        public int? ScheduledRequestId { get; set; }
        
        public string MerchantId { get; set; }
        
        public int? SupplierId { get; set; }
        
        public string Pickupdate { get; set; }
        
        public string Returndate { get; set; }
        
        public int PickupLocationId { get; set; }
        
        public int ReturnLocationId { get; set; }
        
        public int? PickupOfficeId { get; set; }
        
        public int? ReturnOfficeId { get; set; }
        
        public int? SourceMarketId { get; set; }
        
        public string PickupLocationCode { get; set; }
        
        public string ReturnLocationCode { get; set; }
       
    }
}
