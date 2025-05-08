using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Register AzureBlobTaxRatesService with DI
builder.Services.AddSingleton<AzureBlobTaxRatesService>();

// Load configuration from appsettings.json, environment variables, and command-line arguments
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // Ensure this is called to load environment variables

var configuration = builder.Configuration;

// Configure AzureBlobSettings
builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));

// Configure logging
builder.Logging.ClearProviders(); // Optional, if you want to clear previous providers
builder.Logging.AddConsole(); // Add the console logger

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI in development (you can enable for all environments as well)
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Example logging usage
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started at {time}", DateTime.UtcNow);

app.Run();