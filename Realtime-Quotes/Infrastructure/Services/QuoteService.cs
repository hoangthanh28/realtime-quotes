using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IHttpClientFactory httpClientFactory;
        public QuoteService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<GetQuoteForSupplierResult> QuoteRequestForCity(object req)
        {
            DateTime start = DateTime.Now;
            var request = req as QuoteRequest;
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{request.Url}?city={request.CityId}");
            var result = await response.Content.ReadAsAsync<GetQuoteForSupplierResult>();
            result.ResponseTime = DateTime.Now.Subtract(start).TotalMilliseconds;
            result.TaskId = request.TaskId.ToString();
            return result;
        }
    }
}
