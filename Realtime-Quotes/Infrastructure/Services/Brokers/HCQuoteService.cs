using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class HCQuoteService : BaseQuoteService
    {
        private IHttpClientFactory clientFactory;
        public HCQuoteService(IHttpClientFactory clientFactory) : base()
        {
            this.clientFactory = clientFactory;
        }

        protected override HttpClient GetHttpClient()
        {
            return clientFactory.CreateClient("hc");
        }
        protected override int GetMerchantId()
        {
            return 1;
        }
    }
}
