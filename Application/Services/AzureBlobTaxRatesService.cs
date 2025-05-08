using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Identity;  // Ensure this is included
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;  // For IHostEnvironment
using Shared.Models;

namespace Application.Services
{
    public class AzureBlobTaxRatesService
    {
        private readonly AzureBlobSettings _blobSettings;
        private readonly IHostEnvironment _env;

        public AzureBlobTaxRatesService(IOptions<AzureBlobSettings> blobSettings, IHostEnvironment env)
        {
            _blobSettings = blobSettings.Value ?? throw new ArgumentNullException(nameof(blobSettings));
            _env = env;
        }

        public async Task<Dictionary<string, dynamic>> LoadTaxRatesAsync()
        {
            Console.WriteLine("Starting to load tax rates...");

            BlobServiceClient blobServiceClient;

            if (_env.IsDevelopment())
            {
                // Use connection string for local development
                blobServiceClient = new BlobServiceClient(_blobSettings.BlobConnectionString);
            }
            else
            {
                // Use Managed Identity when running in Azure
                blobServiceClient = new BlobServiceClient(new Uri($"https://{_blobSettings.TaxRatesContainerName}.blob.core.windows.net"), new DefaultAzureCredential());
            }

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
