using System.Text.Json;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Application.Services
{
    public class AzureBlobTaxRatesService
    {
        private readonly AzureBlobSettings _blobSettings;

        // Primary constructor that injects IOptions<AzureBlobSettings>
        public AzureBlobTaxRatesService(IOptions<AzureBlobSettings> blobSettings)
        {
            _blobSettings = blobSettings.Value ?? throw new ArgumentNullException(nameof(blobSettings));

            // Debugging - check if settings are loaded correctly
            // Console.WriteLine($"Blob Connection String: {_blobSettings.BlobConnectionString ?? "NOT SET"}");
            // Console.WriteLine($"Blob Container Name: {_blobSettings.ContainerName ?? "NOT SET"}");
            // Console.WriteLine($"Blob Name: {_blobSettings.BlobName ?? "NOT SET"}");
        }

        public async Task<Dictionary<string, dynamic>> LoadTaxRatesAsync()
        {
            // Debugging the method execution
            Console.WriteLine("Starting to load tax rates...");

            var blobServiceClient = new BlobServiceClient(_blobSettings.BlobConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.TaxRatesContainerName);
            var blobClient = containerClient.GetBlobClient(_blobSettings.TaxRatesUkBlobName);

            using var downloadStream = new MemoryStream();
            await blobClient.DownloadToAsync(downloadStream);
            downloadStream.Position = 0;

            using var reader = new StreamReader(downloadStream);
            var jsonContent = await reader.ReadToEndAsync();

            var taxRates = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonContent);
            return taxRates ?? new Dictionary<string, dynamic>();
        }
    }
}