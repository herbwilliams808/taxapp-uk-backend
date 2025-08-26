using System.Net.Http.Headers;
using System.Net.Http.Json;
using Core.Interfaces.Http;

namespace Infrastructure.Http;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string relativeUrl,
        TRequest content,
        string accessToken,
        string tokenType)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        requestMessage.Content = JsonContent.Create(content);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var deserializedResponse = await response.Content.ReadFromJsonAsync<TResponse>();
        return deserializedResponse ?? throw new HttpRequestException("Failed to deserialize the API response. The response body was null or empty.");
    }

    public async Task<string> GetAsync(string relativeUrl)
    {
        var response = await _httpClient.GetAsync(relativeUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetWithAuthAsync(string relativeUrl, string accessToken, string tokenType)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}