using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Hubs
{
    public class SearchHub : Hub
    {
        public async Task OpenSearchRoom(string taskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, taskId);
        }
    }
}
