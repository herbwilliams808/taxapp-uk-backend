using System.Text.Json;
using Application.Services;
using Shared.Models.Settings;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

// Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register services
builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));
builder.Services.AddSingleton<AzureBlobDataLoaderService>();
builder.Services.AddSingleton<JsonDeserializerService>();
builder.Services.AddSingleton<TaxRatesCacheService>();
builder.Services.AddSingleton<TaxEstimationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application startup begins...");

try
{
    var blobLoader = app.Services.GetRequiredService<AzureBlobDataLoaderService>();
    var jsonDeserializer = app.Services.GetRequiredService<JsonDeserializerService>();
    var cacheService = app.Services.GetRequiredService<TaxRatesCacheService>();

    // Step 1: Load raw JSON string from Azure Blob storage
    var jsonString = await blobLoader.LoadBlobDataAsync();

    // Step 2: Parse JSON string into JsonDocument
    using var jsonDoc = jsonDeserializer.Deserialize<JsonDocument>(jsonString);

    // Step 3: Load the tax rates cache with the JSON root element
    cacheService.LoadCache(jsonDoc.RootElement);

    logger.LogInformation("Tax rates cache loaded successfully at startup.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Error during application startup: Failed to load tax rates cache.");
    throw; // Prevent app start if loading tax rates fails
}

// Enable swagger UI only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
