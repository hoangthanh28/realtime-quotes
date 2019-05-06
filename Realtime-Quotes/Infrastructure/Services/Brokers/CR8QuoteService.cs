using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class CR8QuoteService : BaseQuoteService
    {
        private IHttpClientFactory clientFactory;
        public CR8QuoteService(IHttpClientFactory clientFactory) : base()
        {
            this.clientFactory = clientFactory;
        }

        protected override HttpClient GetHttpClient()
        {
            return clientFactory.CreateClient("cr8");
        }
        protected override int GetMerchantId()
        {
            return 12;
        }
    }
}
