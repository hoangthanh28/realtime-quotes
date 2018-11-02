using Microsoft.AspNetCore.SignalR;
using RealtimeQuotes.Infrastructure.Hubs;
using RealtimeQuotes.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class QuotePublisher : IPublisher
    {
        private IHubContext<QuotesHub> quotesHub;
        public QuotePublisher(IHubContext<QuotesHub> quotesHub)
        {
            this.quotesHub = quotesHub;
        }
        public async Task PuslishAsync(string hubName, GetQuoteForSupplierResult result)
        {
            result.Quote = Math.Ceiling(result.Quote);
            await quotesHub.Clients.All.SendCoreAsync("quotePosted", new object[] { result });
        }
    }
}
