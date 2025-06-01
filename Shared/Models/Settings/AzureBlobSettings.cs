namespace Shared.Models.Settings;

/// <summary>
/// Represents the configuration settings for accessing Azure Blob Storage.
/// </summary>
public class AzureBlobSettings
{
    /// <summary>
    /// The connection string to the Azure Blob Storage account.
    /// </summary>
    public string? BlobConnectionString { get; set; }

    /// <summary>
    /// The name of the container where the tax rates JSON blob resides.
    /// </summary>
    public required string TaxRatesContainerName { get; set; }

    /// <summary>
    /// The name of the specific blob containing tax rates data.
    /// </summary>
    public required string TaxRatesUkBlobName { get; set; }
}