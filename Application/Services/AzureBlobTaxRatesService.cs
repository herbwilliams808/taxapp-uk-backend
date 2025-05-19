using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Identity;  // Ensure this is included
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;  // For IHostEnvironment
using Microsoft.Extensions.Logging; // Added for logging
using Shared.Models;
using Shared.Models.Settings;

namespace Application.Services
{
    public class AzureBlobTaxRatesService(
        IOptions<AzureBlobSettings> blobSettings,
        IHostEnvironment env,
        ILogger<AzureBlobTaxRatesService> logger)
    {
        private readonly AzureBlobSettings _blobSettings = blobSettings.Value ?? throw new ArgumentNullException(nameof(blobSettings));

        private readonly ILogger<AzureBlobTaxRatesService> _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize logger
            // Added ILogger
        private Dictionary<string, dynamic>? _cachedTaxRates; // Cache for tax rates

        public async Task<Dictionary<string, dynamic>> LoadTaxRatesAsync()
        {
            if (_cachedTaxRates != null)
            {
                _logger.LogInformation("------| Loading tax rates from cache...");
                return _cachedTaxRates;
            }
            
            _logger.LogInformation("------| Loading tax rates from blob storage...");

            try
            {
                BlobServiceClient blobServiceClient;
                if (env.IsDevelopment())
                {
                    blobServiceClient = new BlobServiceClient(_blobSettings.BlobConnectionString);
                }
                else
                {
                    var uri = new Uri($"https://taxappuksa.blob.core.windows.net");
                    var defaultCredential = new DefaultAzureCredential();
                    blobServiceClient = new BlobServiceClient(uri, defaultCredential);
                }

                var containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.TaxRatesContainerName);
                var blobClient = containerClient.GetBlobClient(_blobSettings.TaxRatesUkBlobName);
                using var downloadStream = new MemoryStream();
                await blobClient.DownloadToAsync(downloadStream);
                downloadStream.Position = 0;
                using var reader = new StreamReader(downloadStream);
                var jsonContent = await reader.ReadToEndAsync();
                _cachedTaxRates = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonContent);
                return _cachedTaxRates ?? new Dictionary<string, dynamic>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "------| Error while loading tax rates.");
                throw;
            }
        }
    }
}
