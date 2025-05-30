using System.Text;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Shared.Models.Settings;

namespace Application.Services
{
    public class AzureBlobDataLoaderService
    {
        private readonly AzureBlobSettings _blobSettings;
        private readonly ILogger<AzureBlobDataLoaderService> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        // Primary constructor for dependency injection, allowing BlobServiceClient to be mocked
        public AzureBlobDataLoaderService(IOptions<AzureBlobSettings> blobSettings, ILogger<AzureBlobDataLoaderService> logger, BlobServiceClient blobServiceClient)
        {
            _blobSettings = blobSettings?.Value ?? throw new ArgumentNullException(nameof(blobSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
        }

        // Secondary constructor for production use, creating BlobServiceClient internally
        public AzureBlobDataLoaderService(IOptions<AzureBlobSettings> blobSettings, ILogger<AzureBlobDataLoaderService> logger)
            : this(
                  blobSettings,
                  logger,
                  CreateBlobServiceClient(blobSettings?.Value) // Call a static helper to create the client
              )
        {
        }

        // Helper method to create BlobServiceClient based on settings
        private static BlobServiceClient CreateBlobServiceClient(AzureBlobSettings? settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "AzureBlobSettings cannot be null when creating BlobServiceClient.");
            }

            if (string.IsNullOrWhiteSpace(settings.BlobConnectionString))
            {
                var blobUri = new Uri("https://taxappuksa.blob.core.windows.net");
                return new BlobServiceClient(blobUri, new DefaultAzureCredential());
            }
            else
            {
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
                _logger.LogInformation("Fetching data from Azure Blob Storage...");

                var containerClient = _blobServiceClient.GetBlobContainerClient(_blobSettings.TaxRatesContainerName);
                var blobClient = containerClient.GetBlobClient(_blobSettings.TaxRatesUkBlobName);

                using var downloadStream = new MemoryStream();
                await blobClient.DownloadToAsync(downloadStream);
                downloadStream.Position = 0;

                using var reader = new StreamReader(downloadStream, Encoding.UTF8);
                var blobContent = await reader.ReadToEndAsync();

                _logger.LogInformation("Data successfully fetched from Azure Blob Storage.");
                return blobContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch data from Azure Blob Storage.");
                throw;
            }
        }
    }
}
