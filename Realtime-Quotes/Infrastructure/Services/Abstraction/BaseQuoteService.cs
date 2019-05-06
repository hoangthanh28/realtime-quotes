using Newtonsoft.Json.Linq;
using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services
{
    public abstract class BaseQuoteService : IQuoteService
    {
        public BaseQuoteService()
        {
        }
        protected abstract HttpClient GetHttpClient();
        public virtual async Task<JObject> QuoteRequestAsync(JObject req)
        {
            DateTime start = DateTime.Now;
            var response = await GetHttpClient().PostAsync($"pw.axd?pricewatchservice.svc/web/GetPrices", new StringContent(req.ToString(), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsAsync<JObject>();
            result["ResponseTime"] = DateTime.Now.Subtract(start).TotalMilliseconds;
            //result.TaskId = request.RoomId.ToString();
            return result;
        }
    }
}
