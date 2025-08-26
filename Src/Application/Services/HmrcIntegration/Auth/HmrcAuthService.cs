using System.Net.Http.Headers;
using System.Net.Http.Json;
using Core.Interfaces.HmrcIntegration.Auth;
using Core.Models.HmrcIntegration.Auth;
using Microsoft.Extensions.Configuration;

// Required for ArgumentNullException

namespace Application.Services.HmrcIntegration.Auth;

public class HmrcAuthService : IHmrcAuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private AccessTokenResponse? _cachedToken;
    private DateTime _tokenExpiryTime;

    public HmrcAuthService(HttpClient httpClient, IConfiguration configuration)
    {
        // Add null checks at the very beginning
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        _httpClient = httpClient;
        // The token URL should ideally be pulled from configuration
        _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("HMRC_TestApi:TokenUrl") ?? "https://test-api.service.hmrc.gov.uk/oauth/token");

        _clientId = configuration.GetValue<string>("HMRC_TestApi:ClientId")
                    ?? throw new InvalidOperationException("HMRC_TestApi:ClientId is not configured.");
        _clientSecret = configuration.GetValue<string>("HMRC_TestApi:ClientSecret")
                        ?? throw new InvalidOperationException("HMRC_TestApi:ClientSecret is not configured.");
    }

    /// <inheritdoc/>
    public async Task<AccessTokenResponse> GetAccessTokenAsync()
    {
        // Check if cached token exists and is still valid
        // Subtract a small buffer (e.g., 60 seconds) to refresh before actual expiry
        if (_cachedToken != null && DateTime.UtcNow < _tokenExpiryTime)
        {
            return _cachedToken;
        }

        // Token is expired or not present, fetch a new one
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            // HMRC API documentation for test users indicates no specific scope is generally required for create-test-user
            // but if other APIs require scopes, you would add them here:
            // new KeyValuePair<string, string>("scope", "write:vat read:vat")
        });

        _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers to avoid conflicts
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.PostAsync("", content); // Post to the base address (token URL)

        response.EnsureSuccessStatusCode(); // Throws an exception for 4xx or 5xx responses

        var tokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new HttpRequestException("Failed to obtain access token or token was empty.");
        }

        // Cache the token and set its expiry time
        _cachedToken = tokenResponse;
        _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60); // Subtract buffer for early refresh

        return _cachedToken;
    }
}