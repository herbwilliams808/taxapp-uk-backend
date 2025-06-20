using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Models.Settings;

namespace Application.Services.HmrcIntegration.Auth
{
    public class AzureBlobTaxRatesService(
        IOptions<AzureBlobSettings> blobSettings,
        IHostEnvironment env,
        ILogger<AzureBlobTaxRatesService> logger)
    {
        private readonly AzureBlobSettings _blobSettings = blobSettings.Value ?? throw new ArgumentNullException(nameof(blobSettings));
        private readonly IHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));
        private readonly ILogger<AzureBlobTaxRatesService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Cache for tax rates with key as string and value as JsonElement to allow dynamic JSON navigation
        private Dictionary<string, JsonElement>? _cachedTaxRates;

        /// <summary>
        /// Loads tax rates from Azure Blob storage or cache.
        /// Expects JSON with a root property "TaxRates" which is an object of key-value pairs.
        /// </summary>
        /// <returns>Dictionary of tax rate keys to JsonElement values</returns>
        public async Task LoadTaxRatesAsync()
        {
            if (_cachedTaxRates != null)
            {
                _logger.LogInformation("------| Tax rates already loaded into cache.");
                return;
            }

            _logger.LogInformation("------| Loading tax rates from blob storage...");

            try
            {
                BlobServiceClient blobServiceClient;

                if (_env.IsDevelopment())
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

                var rootDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent);

                if (rootDictionary != null && rootDictionary.TryGetValue("TaxRates", out var taxRatesElement))
                {
                    _cachedTaxRates = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(taxRatesElement.GetRawText());
                    _logger.LogInformation("------| Tax rates successfully loaded into cache.");
                    return;
                }

                throw new Exception("------| 'TaxRates' property not found in the blob JSON.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "------| Error while loading tax rates.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a tax rate value based on year, region, and property.
        /// </summary>
        /// <param name="year">The year the tax year ends (e.g., 2025 for year_2024_25).</param>
        /// <param name="region">The region (e.g., "england", "scotland"). Optional.</param>
        /// <param name="property">The property name (e.g., "basicRateThreshold").</param>
        /// <returns>The value of the requested tax rate, or null if not found.</returns>
        public object? GetTaxRate(int year, string? region, string property)
        {
            if (_cachedTaxRates == null)
            {
                throw new InvalidOperationException("Tax rates have not been loaded into cache.");
            }

            var taxYearKey = $"year_{year - 1}_{year}";

            if (!_cachedTaxRates.TryGetValue(taxYearKey, out var yearData) || yearData.ValueKind != JsonValueKind.Object)
            {
                _logger.LogWarning("Tax year {TaxYear} not found in cache.", taxYearKey);
                return null; // Tax year not found
            }

            var yearDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(yearData.GetRawText());

            if (yearDict == null)
            {
                _logger.LogWarning("Failed to deserialize data for tax year {TaxYear}.", taxYearKey);
                return null;
            }

            // Attempt to find the property under the specified region
            if (!string.IsNullOrWhiteSpace(region))
            {
                var regionKey = region.ToLower();
                if (yearDict.TryGetValue(regionKey, out var regionData) && regionData.ValueKind == JsonValueKind.Object)
                {
                    var regionDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(regionData.GetRawText());
                    if (regionDict != null && regionDict.TryGetValue(property, out var regionalValue))
                    {
                        return regionalValue.Deserialize<object>(); // Return value found under region
                    }
                }
            }

            // Fallback to property directly under the tax year (no region specified)
            if (yearDict.TryGetValue(property, out var generalValue))
            {
                return generalValue.Deserialize<object>();
            }

            _logger.LogWarning("Property '{Property}' not found for tax year {TaxYear}.", property, taxYearKey);
            return null; // Property not found
        }
    }
}
