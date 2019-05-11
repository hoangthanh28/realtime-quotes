using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class PWCQuoteService : BaseQuoteService
    {
        private IHttpClientFactory clientFactory;
        public PWCQuoteService(IHttpClientFactory clientFactory) : base()
        {
            this.clientFactory = clientFactory;
        }

        protected override HttpClient GetHttpClient()
        {
            return clientFactory.CreateClient("pwc");
        }

        protected override int GetMerchantId()
        {
            return 3;
        }
    }
}
