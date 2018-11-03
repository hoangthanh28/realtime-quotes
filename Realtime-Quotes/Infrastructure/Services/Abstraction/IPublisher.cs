using RealtimeQuotes.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services
{
    public interface IPublisher
    {
        Task PuslishAsync(string taskId, GetQuoteForSupplierResult result);
    }
}
