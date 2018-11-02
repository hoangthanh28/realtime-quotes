using RealtimeQuotes.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services.Abstraction
{
    public interface IBackgroundTaskQueue
    {
        bool HasQueueItem();
        void QueueBackgroundWorkItem(Func<object, Task<GetQuoteForSupplierResult>> workItem, object state);

        Task<Tuple<Func<object, Task<GetQuoteForSupplierResult>>, object>> DequeueAsync(CancellationToken cancellationToken);

        EventHandler<EventArgs> QueueRelease { get; set; }

    }
}
