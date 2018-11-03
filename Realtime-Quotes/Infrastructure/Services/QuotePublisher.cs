using Microsoft.AspNetCore.SignalR;
using RealtimeQuotes.Infrastructure.Hubs;
using RealtimeQuotes.Infrastructure.Models;
using System;
using System.Threading.Tasks;
namespace RealtimeQuotes.Infrastructure.Services
{
    public class QuotePublisher : IPublisher
    {
        private IHubContext<SearchHub> quotesHub;
        public QuotePublisher(IHubContext<SearchHub> quotesHub)
        {
            this.quotesHub = quotesHub;
        }
        public async Task PuslishAsync(string taskId, GetQuoteForSupplierResult result)
        {
            result.Quote = Math.Ceiling(result.Quote);
            await quotesHub.Clients.Group(taskId).SendAsync("quotePosted", result);
        }
    }
}
