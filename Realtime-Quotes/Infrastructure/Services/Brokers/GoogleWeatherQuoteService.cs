using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class GoogleWeatherQuoteService : BaseQuoteService
    {
        public GoogleWeatherQuoteService(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string BrokerName() => "GoogleWeather";
    }
}
