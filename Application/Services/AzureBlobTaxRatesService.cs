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

            try
            {
                if (_env.IsDevelopment())
                {
                    Console.WriteLine("Using connection string for local development...");
                    blobServiceClient = new BlobServiceClient(_blobSettings.BlobConnectionString);
                }
                else
                {
                    Console.WriteLine("Using Managed Identity for Azure environment...");
                    var uri = new Uri($"https://{_blobSettings.TaxRatesContainerName}.blob.core.windows.net");
                    Console.WriteLine($"Blob Service URI: {uri}");
                    blobServiceClient = new BlobServiceClient(uri, new DefaultAzureCredential());
                }

                var containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.TaxRatesContainerName);
                Console.WriteLine($"Container URI: {containerClient.Uri}");

                var blobClient = containerClient.GetBlobClient(_blobSettings.TaxRatesUkBlobName);
                Console.WriteLine($"Blob URI: {blobClient.Uri}");

                using var downloadStream = new MemoryStream();
                await blobClient.DownloadToAsync(downloadStream);
                Console.WriteLine("Blob downloaded successfully.");

                downloadStream.Position = 0;
                using var reader = new StreamReader(downloadStream);
                var jsonContent = await reader.ReadToEndAsync();

                Console.WriteLine("Blob content successfully read. Attempting to deserialize...");
                var taxRates = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonContent);

                Console.WriteLine("Deserialization completed successfully.");
                return taxRates ?? new Dictionary<string, dynamic>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while loading tax rates: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
