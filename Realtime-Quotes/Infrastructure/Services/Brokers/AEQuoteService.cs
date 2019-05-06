using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class AEQuoteService : BaseQuoteService
    {
        private IHttpClientFactory clientFactory;
        public AEQuoteService(IHttpClientFactory clientFactory) : base()
        {
            this.clientFactory = clientFactory;
        }

        protected override HttpClient GetHttpClient()
        {
            return clientFactory.CreateClient("ae");
        }
    }
}
