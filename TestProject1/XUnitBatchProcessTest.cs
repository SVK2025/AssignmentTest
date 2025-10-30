using System.Diagnostics;
using AssignmentTest.Models;
using AssignmentTest.Services;
using Xunit;

namespace TestProject1
{
    public class XUnitBatchProcessTest : IDisposable
    {
        private readonly BatchProcessingService _batchService;

        public XUnitBatchProcessTest()
        {
            _batchService = new BatchProcessingService();
        }

        public void Dispose()
        {
            // Cleanup if needed
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Should_Process_All_Requests_Within_2_Seconds()
        {
            // Arrange
            var tasks = new List<Task<Result>>();
            var sw = Stopwatch.StartNew();

            // Act - Simulate 4 parallel worker threads
            for (int i = 1; i <= 4; i++)
            {
                tasks.Add(_batchService.ProcessSingleAsync(new DataObject
                {
                    Id = i,
                    Input = $"Input_{i}"
                }));
            }

            var results = await Task.WhenAll(tasks);
            sw.Stop();

            // Assert
            Assert.True(sw.Elapsed.TotalSeconds < 2.0, $"Took too long: {sw.Elapsed.TotalSeconds}s");
            Assert.Equal(4, results.Length);
            Assert.All(results, r => Assert.StartsWith("Processed_", r.Output));

            // Verify results order and content
            for (int i = 0; i < results.Length; i++)
            {
                Assert.Equal(i + 1, results[i].Id);
                Assert.Equal($"Processed_Input_{i + 1}", results[i].Output);
            }
        }

        [Fact]
        public async Task Should_Process_Single_Request_Successfully()
        {
            // Arrange
            var data = new DataObject
            {
                Id = 1,
                Input = "SingleTest"
            };

            // Act
            var result = await _batchService.ProcessSingleAsync(data);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Processed_SingleTest", result.Output);
        }

        [Fact]
        public async Task Should_Handle_Multiple_Batches()
        {
            // Arrange
            var tasks = new List<Task<Result>>();
            const int totalRequests = 8; // Will create two batches
            var sw = Stopwatch.StartNew();

            // Act
            for (int i = 1; i <= totalRequests; i++)
            {
                tasks.Add(_batchService.ProcessSingleAsync(new DataObject
                {
                    Id = i,
                    Input = $"Batch_{i}"
                }));
            }

            var results = await Task.WhenAll(tasks);
            sw.Stop();

            // Assert
            Assert.Equal(totalRequests, results.Length);
            Assert.True(sw.Elapsed.TotalSeconds < 4.0, "Processing multiple batches took too long");

            // Verify all results are processed correctly
            for (int i = 0; i < results.Length; i++)
            {
                Assert.Equal(i + 1, results[i].Id);
                Assert.Equal($"Processed_Batch_{i + 1}", results[i].Output);
            }
        }

        [Fact]
        public async Task Should_Timeout_Under_Heavy_Load()
        {
            // Arrange
            var tasks = new List<Task>();
            const int heavyLoadSize = 20;

            // Act & Assert
            for (int i = 0; i < heavyLoadSize; i++)
            {
                var data = new DataObject
                {
                    Id = i,
                    Input = $"Timeout_Test_{i}"
                };

                tasks.Add(Assert.ThrowsAsync<TimeoutException>(() =>
                    _batchService.ProcessSingleAsync(data)));
            }

            await Task.WhenAll(tasks);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Should_Process_Different_Batch_Sizes(int batchSize)
        {
            // Arrange
            var tasks = new List<Task<Result>>();

            // Act
            for (int i = 1; i <= batchSize; i++)
            {
                tasks.Add(_batchService.ProcessSingleAsync(new DataObject
                {
                    Id = i,
                    Input = $"Size_{batchSize}_{i}"
                }));
            }

            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(batchSize, results.Length);
            Assert.All(results, r => Assert.Contains("Processed_", r.Output));

            // Verify order preservation
            for (int i = 0; i < results.Length; i++)
            {
                Assert.Equal(i + 1, results[i].Id);
                Assert.Equal($"Processed_Size_{batchSize}_{i + 1}", results[i].Output);
            }
        }
    }
}
