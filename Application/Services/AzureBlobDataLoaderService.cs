using System; // Required for ArgumentNullException, ArgumentException, Uri
using System.Text;
using Application.Interfaces.Services;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Shared.Models.Settings;
using System.IO; // Required for MemoryStream, StreamReader
using System.Threading.Tasks; // Required for Task

namespace Application.Services
{
    public class AzureBlobDataLoaderService : IBlobDataLoaderService
    {
        private readonly AzureBlobSettings _blobSettings;
        private readonly ILogger<AzureBlobDataLoaderService> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        // Primary constructor for dependency injection, allowing BlobServiceClient to be mocked
        public AzureBlobDataLoaderService(IOptions<AzureBlobSettings> blobSettingsOptions, ILogger<AzureBlobDataLoaderService> logger, BlobServiceClient blobServiceClient)
        {
            // Null checks for injected dependencies
            _blobSettings = blobSettingsOptions?.Value ?? throw new ArgumentNullException(nameof(blobSettingsOptions), "AzureBlobSettings value is null in primary constructor.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient), "BlobServiceClient cannot be null.");

            // ✨ ADDED VALIDATION CHECKS FOR BLOB SETTINGS ✨
            if (string.IsNullOrWhiteSpace(_blobSettings.TaxRatesContainerName))
            {
                throw new ArgumentException("Container name cannot be null or empty in AzureBlobSettings.", nameof(_blobSettings.TaxRatesContainerName));
            }

            if (string.IsNullOrWhiteSpace(_blobSettings.TaxRatesUkBlobName))
            {
                throw new ArgumentException("Blob name cannot be null or empty in AzureBlobSettings.", nameof(_blobSettings.TaxRatesUkBlobName));
            }
            // ✨ END OF ADDED VALIDATION CHECKS ✨

            _logger.LogInformation("AzureBlobDataLoaderService: Primary constructor entered. BlobConnectionString='{BlobConnectionString}', ContainerName='{ContainerName}', BlobName='{BlobName}'.",
                _blobSettings.BlobConnectionString ?? "null", _blobSettings.TaxRatesContainerName, _blobSettings.TaxRatesUkBlobName);
            _logger.LogInformation("AzureBlobDataLoaderService: Attempting to create BlobServiceClient...");
        }

        // Secondary constructor for production use, creating BlobServiceClient internally
        public AzureBlobDataLoaderService(IOptions<AzureBlobSettings> blobSettings, ILogger<AzureBlobDataLoaderService> logger)
            : this(
                  blobSettings,
                  logger,
                  // Pass the logger to the static helper method here!
                  CreateBlobServiceClient(blobSettings?.Value, logger)
              )
        {
            _logger.LogInformation("AzureBlobDataLoaderService: Secondary constructor entered.");
        }

        // Helper method to create BlobServiceClient based on settings
        // Now accepts an ILogger to enable logging within this static method
        private static BlobServiceClient CreateBlobServiceClient(AzureBlobSettings? settings, ILogger logger)
        {
            logger.LogInformation("CreateBlobServiceClient: Entered static helper method.");
            if (settings == null)
            {
                logger.LogError("CreateBlobServiceClient: AzureBlobSettings object received as null. Configuration binding issue suspected.");
                throw new ArgumentNullException(nameof(settings), "AzureBlobSettings cannot be null when creating BlobServiceClient.");
            }

            logger.LogInformation($"CreateBlobServiceClient: settings.BlobConnectionString isNullOrWhiteSpace: {string.IsNullOrWhiteSpace(settings.BlobConnectionString)}");
            logger.LogInformation($"CreateBlobServiceClient: settings.TaxRatesContainerName: {settings.TaxRatesContainerName ?? "null"}");
            logger.LogInformation($"CreateBlobServiceClient: settings.TaxRatesUkBlobName: {settings.TaxRatesUkBlobName ?? "null"}");

            if (string.IsNullOrWhiteSpace(settings.BlobConnectionString))
            {
                // This is the path for Managed Identity
                var blobUri = new Uri("https://taxappuksa.blob.core.windows.net"); // Confirm this URI is correct!
                logger.LogInformation($"CreateBlobServiceClient: Using DefaultAzureCredential with URI: {blobUri}");
                return new BlobServiceClient(blobUri, new DefaultAzureCredential());
            }
            else
            {
                // This is the path for Connection String
                logger.LogInformation($"CreateBlobServiceClient: Using BlobConnectionString: {settings.BlobConnectionString.Substring(0, Math.Min(20, settings.BlobConnectionString.Length))}..."); // Log partial for security
                return new BlobServiceClient(settings.BlobConnectionString);
            }
        }

        /// <summary>
        /// Loads the blob data as a string from Azure Blob Storage.
        /// </summary>
        /// <returns>Content of the blob as a string.</returns>
        public async Task<string> LoadBlobDataAsync()
        {
            try
            {
                _logger.LogInformation("LoadBlobDataAsync: Fetching data from Azure Blob Storage...");

                // Use the correct property names from AzureBlobSettings
                var containerClient = _blobServiceClient.GetBlobContainerClient(_blobSettings.TaxRatesContainerName);
                var blobClient = containerClient.GetBlobClient(_blobSettings.TaxRatesUkBlobName);

                using var downloadStream = new MemoryStream();
                await blobClient.DownloadToAsync(downloadStream);
                downloadStream.Position = 0;

                using var reader = new StreamReader(downloadStream, Encoding.UTF8);
                var blobContent = await reader.ReadToEndAsync();

                _logger.LogInformation("LoadBlobDataAsync: Data successfully fetched from Azure Blob Storage.");
                return blobContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadBlobDataAsync: Failed to fetch data from Azure Blob Storage.");
                throw;
            }
        }
    }
}