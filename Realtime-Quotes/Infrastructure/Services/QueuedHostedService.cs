using Microsoft.Extensions.Hosting;
using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Services
{
    public class QueuedHostedService : BackgroundService
    {
        public IBackgroundTaskQueue TaskQueue { get; }
        public IPublisher publisher;
        private SemaphoreSlim _signal = new SemaphoreSlim(0);
        public QueuedHostedService(IBackgroundTaskQueue taskQueue, IPublisher publisher)
        {
            TaskQueue = taskQueue;
            this.publisher = publisher;
            TaskQueue.QueueRelease += (sender, e) => {
                _signal.Release();
            };
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
           
            var parallelTasks = new List<Task<GetQuoteForSupplierResult>>();
            while (!cancellationToken.IsCancellationRequested)
            {
                await _signal.WaitAsync(cancellationToken);
                while (TaskQueue.HasQueueItem())
                {
                    var (workItem, state) = await TaskQueue.DequeueAsync(cancellationToken);
                    parallelTasks.Add(workItem(state));
                }
                while (parallelTasks.Count > 0)
                {
                    var firstFinishedTask = await Task.WhenAny(parallelTasks);
                    parallelTasks.Remove(firstFinishedTask);
                    var result = await firstFinishedTask;
                    await publisher.PuslishAsync(result.TaskId, result);
                }
            }
        }
    }
}
