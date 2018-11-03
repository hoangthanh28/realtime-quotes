using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace RealtimeQuotes.Infrastructure.Services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private Queue<Tuple<Func<object, Task<GetQuoteForSupplierResult>>, object>> _workItems = new Queue<Tuple<Func<object, Task<GetQuoteForSupplierResult>>, object>>();

        public Action QueueRelease { get; set; }

        public bool HasQueueItem()
        {
            return _workItems.Count != 0;
        }
        public void QueueBackgroundWorkItem(
            Func<object, Task<GetQuoteForSupplierResult>> workItem, object state)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(Tuple.Create(workItem, state));
        }

        public Task<Tuple<Func<object, Task<GetQuoteForSupplierResult>>, object>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            _workItems.TryDequeue(out var workItem);
            return Task.FromResult(workItem);
        }
    }
}
