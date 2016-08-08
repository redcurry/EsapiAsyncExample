using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EsapiAsyncExample.EsapiService
{
    /// <summary>
    /// Represents an object that runs operations asynchronously
    /// on the thread on which it was instantiated.
    /// </summary>
    public class CurrentThreadWorker
    {
        private TaskScheduler _taskScheduler;

        /// <summary>
        /// Initializes an instance of this class.
        /// </summary>
        public CurrentThreadWorker()
        {
            // Create the task scheduler using the dispatcher to
            // ensure that the proper synchronization context exists.
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            });
        }

        /// <summary>
        /// Runs the given action asynchronously.
        /// </summary>
        /// <param name="a">The action to run asynchronously.</param>
        /// <returns>The started task associated with the given action.</returns>
        public Task RunAsync(Action a)
        {
            return Task.Factory.StartNew(a,
                CancellationToken.None, TaskCreationOptions.None, _taskScheduler);
        }

        /// <summary>
        /// Runs the given function asynchronously.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="f">The function to run asynchronously.</param>
        /// <returns>The started task associated with the given function.</returns>
        public Task<T> RunAsync<T>(Func<T> f)
        {
            return Task.Factory.StartNew(f,
                CancellationToken.None, TaskCreationOptions.None, _taskScheduler);
        }
    }
}
