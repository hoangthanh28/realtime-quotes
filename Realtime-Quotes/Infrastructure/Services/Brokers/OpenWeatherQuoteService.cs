using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class OpenWeatherQuoteService : BaseQuoteService
    {
        public OpenWeatherQuoteService(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string BrokerName() => "OpenWeather";
    }
}
