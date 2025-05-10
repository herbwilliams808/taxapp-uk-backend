using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure logging FIRST
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddAzureWebAppDiagnostics();  // Required for Azure Filesystem/Blob logging
builder.Logging.SetMinimumLevel(LogLevel.Trace); // Or your desired level



// Now get a logger from the host's logging system
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
logger.LogInformation("My Program.cs logger ready :)");
logger.LogInformation("My Program.cs > init builder...");

// Register Services with DI
builder.Services.AddSingleton<AzureBlobTaxRatesService>();
builder.Services.AddSingleton<TaxEstimationService>();

// Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configure AzureBlobSettings
builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI in development
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

logger.LogInformation("Application started at {time}", DateTime.UtcNow);

app.Run();