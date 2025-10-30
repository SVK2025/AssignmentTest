using AssignmentTest.Models;

namespace AssignmentTest.Services
{
    /// <summary>
    /// Singleton service that handles the actual data processing operations.
    /// Simulates GPU computation with thread-safe processing capabilities.
    /// </summary>
    public sealed class DataProcessor
    {
        // Singleton instance using lazy initialization
        private static readonly Lazy<DataProcessor> _instance = new(() => new DataProcessor());
        
        // Lock object for thread-safe processing
     private static readonly object _lock = new();

        /// <summary>
        /// Gets the singleton instance of DataProcessor
    /// </summary>
        public static DataProcessor Instance => _instance.Value;

        // Private constructor to enforce singleton pattern
        private DataProcessor() { }

        /// <summary>
        /// Processes an array of data objects in a thread-safe manner.
        /// Simulates GPU computation with a 1-second delay.
        /// </summary>
      /// <param name="data">Array of data objects to process</param>
      /// <returns>Array of processing results</returns>
    public Result[] ProcessData(DataObject[] data)
{
     lock (_lock) // Ensure non-reentrant protection
      {
       // Simulate GPU computation time
         Thread.Sleep(1000);
            
       // Process each data object and return results
    return data.Select(d => new Result
        {
                  Id = d.Id,
  Output = $"Processed_{d.Input}"
  }).ToArray();
     }
        }
    }
}
