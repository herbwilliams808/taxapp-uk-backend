namespace Application.Interfaces.Services;

public interface IBlobDataLoaderService
{
    /// <summary>
    /// Loads the blob data as a string from Azure Blob Storage.
    /// </summary>
    /// <returns>Content of the blob as a string.</returns>
    Task<string> LoadBlobDataAsync();
}