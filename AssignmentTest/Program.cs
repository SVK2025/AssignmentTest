using AssignmentTest.Services;
using Microsoft.OpenApi.Models;

// Create and configure the web application builder
var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddSingleton<BatchProcessingService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI with detailed information
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Batch Processing API", 
 Version = "v1",
Description = "API for batch processing operations with optimized performance",
        Contact = new OpenApiContact
     {
            Name = "API Support",
      Email = "support@example.com"
  }
    });

    // Add XML comments if you have them configured
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});

// Add logging capabilities
builder.Logging.AddConsole();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable developer exception page in development
  app.UseDeveloperExceptionPage();
    
    // Enable Swagger UI in development with custom configuration
    app.UseSwagger(c =>
    {
   c.SerializeAsV2 = false; // Use OpenAPI 3.0
    });
    
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Batch Processing API V1");
        c.RoutePrefix = "swagger";
      c.DocumentTitle = "Batch Processing API Documentation";
        c.DefaultModelsExpandDepth(2);
        c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
        c.DisplayRequestDuration();
  c.EnableDeepLinking();
        c.EnableFilter();
    });
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Configure routing
app.UseRouting();

// Enable authorization
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Add health check endpoint
app.MapGet("/health", () => "API is running!");

// Start the application
app.Run();

// Make the Program class public for testing
public partial class Program { }
