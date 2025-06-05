using Swashbuckle.AspNetCore.Filters;
using API.SwaggerExamples;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Services;
using Shared.Models.Settings; // Ensure this is the correct namespace for AzureBlobSettings
// For BlobServiceClient
// For DefaultAzureCredential

// For IOptions

var builder = WebApplication.CreateBuilder(args);

// --- FIX START: Correct Logging Configuration ---
// Crucially, remove builder.Logging.ClearProviders() from the main logging setup.
// WebApplication.CreateBuilder() already adds default providers.
builder.Logging.AddAzureWebAppDiagnostics(); // This is essential for App Service filesystem logs
builder.Logging.SetMinimumLevel(LogLevel.Trace); // Set to Trace for maximum verbosity during debugging
// --- FIX END: Correct Logging Configuration ---

// Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // Ensures Azure App Service settings override appsettings.json

// Register services
builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));

// Register AzureBlobDataLoaderService as a Singleton.
// Since AzureBlobDataLoaderService has two constructors, and we want to use the one
// that creates BlobServiceClient internally for production/Azure, we just register it directly.
// DI will prefer the most specific constructor it can satisfy.
// Ensure BlobServiceClient is NOT directly registered as a service here if you rely on the
// AzureBlobDataLoaderService's secondary constructor to create it internally.
builder.Services.AddSingleton<AzureBlobDataLoaderService>();

builder.Services.AddSingleton<JsonDeserializerService>();
builder.Services.AddSingleton<TaxRatesCacheService>();
builder.Services.AddSingleton<TaxEstimationService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<TaxEstimationRequestExample>();

var app = builder.Build();

// --- STARTUP LOGGING AND TAX RATES LOADING ---
// Retrieve a logger instance for startup actions
var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();

startupLogger.LogInformation("### PROGRAM.CS: APPLICATION STARTUP LOG: Trying to log to Azure filesystem. ###"); // Your explicit test log
startupLogger.LogInformation("PROGRAM.CS: Application startup begins...");

try
{
    // Retrieve services directly for startup tasks (ensure they are singletons or scoped appropriately)
    var blobLoader = app.Services.GetRequiredService<AzureBlobDataLoaderService>();
    var jsonDeserializer = app.Services.GetRequiredService<JsonDeserializerService>();
    var cacheService = app.Services.GetRequiredService<TaxRatesCacheService>();

    startupLogger.LogInformation("PROGRAM.CS: Attempting to load tax rates from blob at startup.");
    // Step 1: Load raw JSON string from Azure Blob storage
    var jsonString = await blobLoader.LoadBlobDataAsync();
    startupLogger.LogInformation($"PROGRAM.CS: Successfully loaded {jsonString.Length} bytes from blob storage.");

    // Step 2: Parse JSON string into JsonDocument
    using var jsonDoc = jsonDeserializer.Deserialize<JsonDocument>(jsonString);
    startupLogger.LogInformation("PROGRAM.CS: Successfully deserialized JSON document.");

    // Step 3: Load the tax rates cache with the JSON root element
    cacheService.LoadCache(jsonDoc.RootElement);

    startupLogger.LogInformation("PROGRAM.CS: Tax rates cache loaded successfully at startup.");
}
catch (Exception ex)
{
    startupLogger.LogError(ex, "PROGRAM.CS: FATAL ERROR during application startup: Failed to load tax rates cache. Application will terminate.");
    // Re-throw the exception to prevent the application from starting if critical data is missing
    throw;
}
// --- END STARTUP LOGGING AND TAX RATES LOADING ---


// Enable swagger UI only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();