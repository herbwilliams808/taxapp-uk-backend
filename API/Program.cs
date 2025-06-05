using Swashbuckle.AspNetCore.Filters;
using API.SwaggerExamples;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Calculators;
using Application.Interfaces.Calculators;
using Application.Interfaces.Services;
using Application.Services;
using Shared.Models.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddAzureWebAppDiagnostics();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

// Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // Ensures Azure App Service settings override appsettings.json

// Register services
builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));

builder.Services.AddSingleton<IBlobDataLoaderService, AzureBlobDataLoaderService>();
builder.Services.AddSingleton<IJsonDeserialiserService, JsonDeserialiserService>();
builder.Services.AddSingleton<ITaxRatesCacheService, TaxRatesCacheService>();

builder.Services.AddScoped<ITotalEmploymentIncomeCalculator, TotalEmploymentIncomeCalculator>();
builder.Services.AddScoped<ITotalEmploymentBenefitsCalculator, TotalEmploymentBenefitsCalculator>();
builder.Services.AddScoped<ITotalEmploymentExpensesCalculator, TotalEmploymentExpensesCalculator>();
builder.Services.AddScoped<ITotalOtherDeductionsCalculator, TotalOtherDeductionsCalculator>();
builder.Services.AddScoped<IProfitFromPropertiesCalculator, ProfitFromPropertiesCalculator>();
builder.Services.AddScoped<ITotalIncomeCalculator, TotalIncomeCalculator>();
builder.Services.AddScoped<IGiftAidPaymentsCalculator, GiftAidPaymentsCalculator>();
builder.Services.AddScoped<IBasicRateLimitCalculator, BasicRateLimitCalculator>();
builder.Services.AddScoped<TaxEstimationService>();


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
    var blobLoader = app.Services.GetRequiredService<IBlobDataLoaderService>();
    var jsonDeserializer = app.Services.GetRequiredService<IJsonDeserialiserService>();
    var cacheService = app.Services.GetRequiredService<ITaxRatesCacheService>();

    startupLogger.LogInformation("PROGRAM.CS: Attempting to load tax rates from blob at startup.");
    // Step 1: Load raw JSON string from Azure Blob storage
    var jsonString = await blobLoader.LoadBlobDataAsync();
    startupLogger.LogInformation($"PROGRAM.CS: Successfully loaded {jsonString.Length} bytes from blob storage.");

    // Step 2: Parse JSON string into JsonDocument
    using var jsonDoc = jsonDeserializer.Deserialise<JsonDocument>(jsonString);
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