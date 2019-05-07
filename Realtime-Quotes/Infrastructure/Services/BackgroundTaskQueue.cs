using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace RealtimeQuotes.Infrastructure.Services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private const int MAX_THREADS = 100;
        private readonly BlockingCollection<Tuple<Func<object, Task>, object>>[] _messageQueues = new BlockingCollection<Tuple<Func<object, Task>, object>>[MAX_THREADS];
        private readonly Thread[] _processingThreads = new Thread[MAX_THREADS];
        private readonly object _syncObject = new object();
        private int _counter;

        public BackgroundTaskQueue()
        {

            for (int i = 0; i < MAX_THREADS; i++)
            {
                this._messageQueues[i] = new BlockingCollection<Tuple<Func<object, Task>, object>>();
                this._processingThreads[i] = new Thread(ProcessQueue)
                {
                    IsBackground = true
                };
                this._processingThreads[i].Start(i);
            }
        }

        public void QueueBackgroundWorkItem(Func<object, Task> workItem, object state = null)
        {
            lock (_syncObject)
            {
                _counter = (_counter + 1) % MAX_THREADS;
                _messageQueues[_counter].Add(Tuple.Create(workItem, state));
            }
        }

        private void ProcessQueue(object state)
        {
            var items = _messageQueues[(int)state].GetConsumingEnumerable();

            foreach (var item in items)
            {
                var task = item.Item1(item.Item2);

                try
                {
                    //task.RunSynchronously();
                    Task.Factory.StartNew(() => task, creationOptions: TaskCreationOptions.LongRunning | TaskCreationOptions.RunContinuationsAsynchronously);
                }
                catch (Exception exc)
                {

                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (Thread thread in _processingThreads)
                    {
                        thread.Abort();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
