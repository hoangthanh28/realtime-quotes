using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services
{
    public abstract class BaseQuoteService : IQuoteService
    {
        protected readonly HttpClient httpClient;
        public BaseQuoteService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        protected abstract string BrokerName();
        public virtual async Task<GetQuoteForSupplierResult> QuoteRequestForCity(object req)
        {
            DateTime start = DateTime.Now;
            var request = req as QuoteRequest;
            var response = await httpClient.GetAsync($"/api/{BrokerName()}?city={request.CityId}");
            var result = await response.Content.ReadAsAsync<GetQuoteForSupplierResult>();
            result.ResponseTime = DateTime.Now.Subtract(start).TotalMilliseconds;
            result.TaskId = request.RoomId.ToString();
            return result;
        }
    }
}
