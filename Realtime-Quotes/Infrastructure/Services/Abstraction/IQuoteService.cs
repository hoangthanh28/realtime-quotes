using RealtimeQuotes.Infrastructure.Models;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services.Abstraction
{
    public interface IQuoteService
    {
        Task<GetQuoteForSupplierResult> QuoteRequestForCity(object request);
    }
}
