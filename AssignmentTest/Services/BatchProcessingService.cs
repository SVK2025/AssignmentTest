using AssignmentTest.Models;

namespace AssignmentTest.Services
{
    /// <summary>
    /// Service responsible for batch processing of data objects with optimized performance.
    /// Implements a batching mechanism that processes items in groups for better GPU utilization.
    /// </summary>
    public class BatchProcessingService
    {
        // Lock object for thread-safe operations
        private readonly object _syncLock = new();
   
        // Queue for pending data objects and their completion sources
        private readonly List<(DataObject data, TaskCompletionSource<Result> tcs)> _pending = new();
    
        // Timer to periodically check and process pending items
        private readonly Timer _timer;

        /// <summary>
        /// Initializes a new instance of BatchProcessingService.
        /// Sets up a timer to check for pending work every 200ms.
        /// </summary>
        public BatchProcessingService()
        {
            // Timer runs every 200 ms to check for pending work
            _timer = new Timer(ProcessIfNeeded, null, 200, 200);
        }

        /// <summary>
        /// Processes a single data object asynchronously.
        /// Items are batched for optimal processing when possible.
        /// </summary>
        /// <param name="data">The data object to process</param>
        /// <returns>Task containing the processing result or throws TimeoutException after 2 seconds</returns>
        public Task<Result> ProcessSingleAsync(DataObject data)
        {
            var tcs = new TaskCompletionSource<Result>();

            lock (_syncLock)
            {
                _pending.Add((data, tcs));

                // Trigger immediate processing if batch size threshold is reached
                if (_pending.Count >= 4)
                {
                    _ = Task.Run(ProcessBatch);
                }
            }

            // Timeout safeguard (max 2 seconds)
            return Task.WhenAny(tcs.Task, Task.Delay(2000))
                       .ContinueWith(task =>
                       {
                           if (tcs.Task.IsCompletedSuccessfully)
                               return tcs.Task.Result;
                           throw new TimeoutException("Result took longer than 2 seconds.");
                       });
        }

        /// <summary>
        /// Timer callback that checks for pending items and processes them if any exist.
        /// </summary>
        private void ProcessIfNeeded(object state)
        {
            lock (_syncLock)
            {
                if (_pending.Count > 0)
                {
                    _ = Task.Run(ProcessBatch);
                }
            }
        }

        /// <summary>
        /// Processes all pending items in the current batch.
        /// Thread-safe method that handles the actual processing of data objects.
        /// </summary>
        private void ProcessBatch()
        {
            (DataObject data, TaskCompletionSource<Result> tcs)[] batch;

            lock (_syncLock)
            {
                if (_pending.Count == 0)
                    return;

                // Get all pending items and clear the queue
                batch = _pending.ToArray();
                _pending.Clear();
            }

            // Process all items in the batch
            var results = DataProcessor.Instance.ProcessData(batch.Select(x => x.data).ToArray());
            
            // Set results for all completed items
            for (int i = 0; i < batch.Length; i++)
                batch[i].tcs.TrySetResult(results[i]);
        }
    }
}

