using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class CTQuoteService : BaseQuoteService
    {
        private IHttpClientFactory clientFactory;
        public CTQuoteService(IHttpClientFactory clientFactory) : base()
        {
            this.clientFactory = clientFactory;
        }

        protected override HttpClient GetHttpClient()
        {
            return clientFactory.CreateClient("ct");
        }
        protected override int GetMerchantId()
        {
            return 9;
        }
    }
}
