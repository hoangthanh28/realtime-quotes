using System.Net.Http;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class YahooWeatherQuoteService : BaseQuoteService
    {
        public YahooWeatherQuoteService(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string BrokerName() => "YahooWeather";
    }
}
