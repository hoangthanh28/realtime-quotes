using Newtonsoft.Json.Linq;
using RealtimeQuotes.Infrastructure.Models;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services.Abstraction
{
    public interface IQuoteService
    {
        Task<JObject> QuoteRequestAsync(JObject request);
    }
}
