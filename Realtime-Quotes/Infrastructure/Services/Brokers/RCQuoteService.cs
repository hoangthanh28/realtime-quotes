using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class RCQuoteService : BaseQuoteService
    {
        private IHttpClientFactory clientFactory;
        public RCQuoteService(IHttpClientFactory clientFactory) : base()
        {
            this.clientFactory = clientFactory;
        }

        protected override HttpClient GetHttpClient()
        {
            return clientFactory.CreateClient("rc");
        }
        protected override int GetMerchantId()
        {
            return 8;
        }
    }
}
