using AssignmentTest.Models;
using AssignmentTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AssignmentTest.Controllers
{
  /// <summary>
    /// API controller for handling data processing requests.
    /// Provides endpoints for batch processing operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
   private readonly BatchProcessingService _batchService;
        private readonly ILogger<ProcessController> _logger;

        /// <summary>
        /// Initializes a new instance of ProcessController
        /// </summary>
        /// <param name="batchService">Service for batch processing</param>
        /// <param name="logger">Logger for diagnostic information</param>
        public ProcessController(BatchProcessingService batchService, ILogger<ProcessController> logger)
      {
        _batchService = batchService ?? throw new ArgumentNullException(nameof(batchService));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

   /// <summary>
        /// Processes a single data object asynchronously
     /// </summary>
    /// <param name="data">The data object to process</param>
        /// <returns>ActionResult containing the processing result</returns>
      /// <response code="200">Returns the processed result</response>
        /// <response code="400">If the input data is invalid</response>
  /// <response code="503">If processing times out</response>
[HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<Result>> Post([FromBody] DataObject data)
        {
 try
        {
      // Validate input
    if (data == null)
                {
                  _logger.LogError("Request body is null");
      return BadRequest("Request body cannot be null");
             }

          if (string.IsNullOrEmpty(data.Input))
  {
        _logger.LogError("Input string is null or empty");
                    return BadRequest("Input string cannot be null or empty");
    }

       // Process the request
              _logger.LogInformation("Processing request for ID: {Id}", data.Id);
       var result = await _batchService.ProcessSingleAsync(data);
         _logger.LogInformation("Successfully processed request for ID: {Id}", data.Id);
   
             return Ok(result);
            }
   catch (TimeoutException ex)
       {
_logger.LogError(ex, "Request timed out for ID: {Id}", data?.Id);
       return StatusCode(StatusCodes.Status503ServiceUnavailable, "Processing timed out");
    }
            catch (Exception ex)
       {
         _logger.LogError(ex, "Error processing request for ID: {Id}", data?.Id);
    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred processing the request");
            }
        }

        /// <summary>
        /// Health check endpoint to verify the controller is functioning
        /// </summary>
        /// <returns>OK result with status message</returns>
      [HttpGet("health")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Health()
        {
            return Ok("ProcessController is healthy");
   }
    }
}
