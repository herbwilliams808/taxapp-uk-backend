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

// Inspect all configuration keys and values
// Console.WriteLine("Configuration Values:");
// foreach (var kvp in builder.Configuration.AsEnumerable())
// {
//     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
// }

// Log Azure Blob settings during startup
// var connectionString = configuration["AzureBlobSettings:BlobConnectionString"];
// var containerName = configuration["AzureBlobSettings:ContainerName"];
// var blobName = configuration["AzureBlobSettings:BlobName"];
//
// Console.WriteLine("");
// Console.WriteLine("Blob Variable Values:");
// Console.WriteLine($"Blob Connection String: {connectionString ?? "NOT SET"}");
// Console.WriteLine($"Blob Container Name: {containerName ?? "NOT SET"}");
// Console.WriteLine($"Blob Name: {blobName ?? "NOT SET"}");

// Configure application services

// Add other services like controllers, etc.

// Configure AzureBlobSettings
builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.MapControllers();

app.Run();