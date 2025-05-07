namespace Shared.Models;

/// <summary>
/// Represents the configuration settings for accessing Azure Blob Storage.
/// </summary>
public class AzureBlobSettings
{
    /// <summary>
    /// The connection string to the Azure Blob Storage account.
    /// </summary>
    public required string BlobConnectionString { get; set; }

    /// <summary>
    /// The name of the container where the tax rates JSON blob resides.
    /// </summary>
    public required string ContainerName { get; set; }

    /// <summary>
    /// The name of the specific blob containing tax rates data.
    /// </summary>
    public required string BlobName { get; set; }
}