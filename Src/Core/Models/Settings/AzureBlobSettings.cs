namespace Core.Models.Settings;

public class AzureBlobSettings
{
    public string? BlobConnectionString { get; set; }

    public required string TaxRatesContainerName { get; set; }

    public required string TaxRatesUkBlobName { get; set; }
}