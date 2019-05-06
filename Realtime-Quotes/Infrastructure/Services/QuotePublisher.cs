using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
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
        public Task PuslishAsync(string taskId, JObject result)
        {
            //result.Quote = Math.Ceiling(result.Quote);
            return quotesHub.Clients.Group(taskId).SendAsync("quotePosted", result);
        }
    }
}
