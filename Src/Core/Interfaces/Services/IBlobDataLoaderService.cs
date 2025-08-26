namespace Core.Interfaces.Services;

public interface IBlobDataLoaderService
{
    Task<string> LoadBlobDataAsync();
}