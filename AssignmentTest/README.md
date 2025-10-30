# Batch Processing API Solution

A high-performance .NET 9 solution demonstrating optimized batch processing capabilities through a Web API. This solution showcases modern C# 13.0 features and best practices for handling concurrent processing requests with GPU optimization.

## Solution Structure

```
AssignmentTest/
??? AssignmentTest/  # Main API Project
?   ??? Controllers/
?   ?   ??? ProcessController.cs # API endpoints implementation
?   ??? Models/
?   ?   ??? DataObject.cs       # Input model with Id and Input string
?   ?   ??? Result.cs        # Output model with Id and processed Output
?   ??? Services/
?   ?   ??? BatchProcessingService.cs # Core batching logic
?   ?   ??? DataProcessor.cs    # Data processing implementation
?   ??? Properties/
?     ??? launchSettings.json # API launch configuration
??? TestProject1/  # Test Project
    ??? ProcessControllerTests.cs # API endpoint tests
```

## Technical Stack

- **.NET 9.0**: Latest .NET version with enhanced performance
- **C# 13.0**: Utilizing modern language features
- **ASP.NET Core Web API**: For RESTful endpoint implementation
- **Swagger/OpenAPI 3.0**: API documentation and testing
- **xUnit**: Testing framework for unit tests

## Key Features

### API Endpoints

#### POST /api/Process
Processes data objects with automatic batching optimization.

**Request Model:**
```json
{
    "id": int,
    "input": "string"
}
```

**Response Model:**
```json
{
    "id": int,
    "output": "string"
}
```

**Status Codes:**
- `200 OK`: Successful processing
- `400 Bad Request`: Invalid input
- `503 Service Unavailable`: Processing timeout

#### GET /health
Health check endpoint returning API status.

### Performance Optimizations

1. **Smart Batching**
   - Automatic batching of requests
   - Optimal batch size of 4 items
   - 200ms processing intervals

2. **Concurrency Handling**
   - Thread-safe operations
   - Async/await patterns
   - Request timeout protection (2 seconds)

3. **Resource Management**
   - Singleton batch processing service
   - Efficient memory usage
   - GPU utilization optimization

## Getting Started

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 or later (recommended)

### Setup and Run

1. **Clone the Repository**
   ```bash
   git clone [repository-url]
   cd AssignmentTest
   ```

2. **Build the Solution**
   ```bash
   dotnet build
   ```

3. **Run the API**
   ```bash
 cd AssignmentTest
   dotnet run
   ```

4. **Access Swagger Documentation**
   - Development: https://localhost:5001/swagger
   - API Documentation with interactive testing interface

### Running Tests

```bash
cd TestProject1
dotnet test
```

## Development Notes

- API runs in HTTPS by default
- Swagger UI available in development environment
- Console logging enabled for diagnostics
- XML documentation support ready (commented in Program.cs)

## API Documentation

The API is fully documented using Swagger/OpenAPI 3.0 with:
- Detailed endpoint descriptions
- Request/response models
- Status codes and error responses
- Interactive testing interface

## Error Handling

- Developer exception page in development
- Proper HTTP status codes
- Timeout protection for long-running operations
- Input validation

## Configuration

Launch settings and API configuration can be modified in:
- `launchSettings.json`
- `Program.cs` for service configuration
- Swagger settings in Program.cs for API documentation

## Contact

For API support, contact:
- Email: support@example.com

## License

[Specify your license here]