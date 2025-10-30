using System.Net;
using AssignmentTest.Models;
using AssignmentTest.Services;
using AssignmentTest.Controllers;
using Microsoft.Extensions.Logging;
using Xunit;

namespace TestProject1
{
    public class ProcessControllerTests : IDisposable
    {
        private readonly ProcessController _controller;
      private readonly BatchProcessingService _batchService;
        private readonly ILogger<ProcessController> _logger;

     public ProcessControllerTests()
        {
   _batchService = new BatchProcessingService();
        _logger = LoggerFactory.Create(builder => builder.AddConsole())
       .CreateLogger<ProcessController>();
   _controller = new ProcessController(_batchService, _logger);
        }

        public void Dispose()
        {
         GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Post_ShouldProcessDataSuccessfully()
        {
      // Arrange
            var data = new DataObject
        {
                Id = 1,
            Input = "TestInput"
         };

         // Act
            var actionResult = await _controller.Post(data);

      // Assert
            Assert.NotNull(actionResult);
var result = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(actionResult.Result);
      var processedResult = Assert.IsType<Result>(result.Value);
          Assert.Equal(1, processedResult.Id);
  Assert.Equal("Processed_TestInput", processedResult.Output);
        }

[Fact]
   public async Task Post_WithNullData_ShouldReturnBadRequest()
      {
     // Act
 var actionResult = await _controller.Post(null);

    // Assert
   Assert.NotNull(actionResult);
   Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task Post_WithEmptyInput_ShouldReturnBadRequest()
        {
  // Arrange
            var data = new DataObject
            {
    Id = 1,
           Input = string.Empty
        };

            // Act
var actionResult = await _controller.Post(data);

         // Assert
            Assert.NotNull(actionResult);
    Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(actionResult.Result);
}
    }
}