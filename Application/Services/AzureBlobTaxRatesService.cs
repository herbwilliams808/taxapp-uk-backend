using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Identity;  // Ensure this is included
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;  // For IHostEnvironment
using Microsoft.Extensions.Logging; // Added for logging
using Shared.Models;

namespace Application.Services
{
    public class AzureBlobTaxRatesService
    {
        private readonly AzureBlobSettings _blobSettings;
        private readonly IHostEnvironment _env;
        private readonly ILogger<AzureBlobTaxRatesService> _logger; // Added ILogger
        private Dictionary<string, dynamic>? _cachedTaxRates; // Cache for tax rates

        public AzureBlobTaxRatesService(IOptions<AzureBlobSettings> blobSettings, IHostEnvironment env, ILogger<AzureBlobTaxRatesService> logger)
        {
            _blobSettings = blobSettings.Value ?? throw new ArgumentNullException(nameof(blobSettings));
            _env = env;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize logger
            
            // Log the configuration values for debugging
            _logger.LogInformation("------| Blob Connection String: {BlobConnectionString}", _blobSettings.BlobConnectionString);
            _logger.LogInformation("------| Blob Container Name: {TaxRatesContainerName}", _blobSettings.TaxRatesContainerName);
            _logger.LogInformation("------| Blob Name: {TaxRatesUkBlobName}", _blobSettings.TaxRatesUkBlobName);

            _logger.LogInformation("------| Logger initialised");

        }

        public async Task<Dictionary<string, dynamic>> LoadTaxRatesAsync()
        {
            if (_cachedTaxRates != null)
            {
                _logger.LogInformation("------| Returning cached tax rates.");
                return _cachedTaxRates;
            }
            
            _logger.LogInformation("------| Starting to load tax rates...");

            BlobServiceClient blobServiceClient;

            try
            {
                if (_env.IsDevelopment())
                {
                    _logger.LogInformation("------| Using connection string for local development...");
                    blobServiceClient = new BlobServiceClient(_blobSettings.BlobConnectionString);
                }
                else
                {
                    _logger.LogInformation("------| Using Managed Identity for Azure environment...");
                    var uri = new Uri($"https://taxappuksa.blob.core.windows.net");
                    _logger.LogInformation($"------| Blob Service URI: {uri}");
                    
                    _logger.LogInformation("------| Attempting to authenticate using Managed Identity...");
                    var defaultCredential = new DefaultAzureCredential();
                    _logger.LogInformation("------| Managed Identity authentication successful.");

                    blobServiceClient = new BlobServiceClient(uri, defaultCredential);
                    _logger.LogInformation("------| Managed Identity authentication successful.");
                }

                var containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.TaxRatesContainerName);
                _logger.LogInformation($"------| Container URI: {containerClient.Uri}");

                var blobClient = containerClient.GetBlobClient(_blobSettings.TaxRatesUkBlobName);
                _logger.LogInformation($"------| Blob URI: {blobClient.Uri}");

                using var downloadStream = new MemoryStream();
                await blobClient.DownloadToAsync(downloadStream);
                _logger.LogInformation("------| Blob downloaded successfully.");

                downloadStream.Position = 0;
                using var reader = new StreamReader(downloadStream);
                var jsonContent = await reader.ReadToEndAsync();

                _cachedTaxRates = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonContent);
                _logger.LogInformation("------| Tax rates deserialized successfully.");
                return _cachedTaxRates ?? new Dictionary<string, dynamic>();

                // _logger.LogInformation("------| Blob content successfully read. Attempting to deserialize...");
                // var taxRates = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonContent);
                //
                // _logger.LogInformation("------| Deserialization completed successfully.");
                // return taxRates ?? new Dictionary<string, dynamic>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "------| Error while loading tax rates.");
                throw;
            }
        }
    }
}
