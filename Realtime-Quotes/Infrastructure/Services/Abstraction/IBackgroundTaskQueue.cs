using RealtimeQuotes.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services.Abstraction
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<object, Task> data, object state = null);

    }
}
