using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class WorldWeatherQuoteService : BaseQuoteService
    {
        public WorldWeatherQuoteService(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string BrokerName() => "WorldWeather";
    }
}
