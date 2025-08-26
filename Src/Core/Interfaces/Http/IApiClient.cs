namespace Core.Interfaces.Http;

public interface IApiClient
{
    Task<TResponse> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest content, string accessToken, string tokenType);

    Task<string> GetAsync(string relativeUrl);

    Task<string> GetWithAuthAsync(string relativeUrl, string accessToken, string tokenType);
}