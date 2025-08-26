using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using Core.Interfaces.HmrcIntegration.Auth;
using Core.Models.HmrcIntegration.Auth;
using Microsoft.Extensions.Configuration;

// For HttpUtility.ParseQueryString and HtmlEncode

// For ConcurrentDictionary for in-memory token storage

namespace Application.Services.HmrcIntegration.Auth;

public class HmrcUserAuthService : IHmrcUserAuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUri;
    private readonly string _authBaseUrl;
    private readonly string _tokenUrl;

    // For simplicity in a tutorial, we'll store user tokens in-memory.
    // In a real application, this would be backed by a secure, persistent store (e.g., database, distributed cache).
    private static readonly ConcurrentDictionary<string, AccessTokenResponse> _userTokensCache = new();

    public HmrcUserAuthService(HttpClient httpClient, IConfiguration configuration)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        _httpClient = httpClient;
        _clientId = configuration.GetValue<string>("HMRC_TestApi:ClientId")
                    ?? throw new InvalidOperationException("HMRC_TestApi:ClientId is not configured.");
        _clientSecret = configuration.GetValue<string>("HMRC_TestApi:ClientSecret")
                        ?? throw new InvalidOperationException("HMRC_TestApi:ClientSecret is not configured.");
        _redirectUri = configuration.GetValue<string>("HMRC_TestApi:RedirectUri")
                       ?? throw new InvalidOperationException("HMRC_TestApi:RedirectUri is not configured.");
        _authBaseUrl = configuration.GetValue<string>("HMRC_TestApi:AuthBaseUrl")
                       ?? throw new InvalidOperationException("HMRC_TestApi:AuthBaseUrl is not configured.");
        _tokenUrl = configuration.GetValue<string>("HMRC_TestApi:TokenUrl")
                    ?? throw new InvalidOperationException("HMRC_TestApi:TokenUrl is not configured.");
    }

    /// <inheritdoc/>
    public string GetAuthorizationUrl(string scopes, string state)
    {
        // For the "Hello User!" endpoint, the scope is typically "hello" or "hello-user"
        // Consult HMRC documentation for exact scopes required for each API.
        var uriBuilder = new UriBuilder(_authBaseUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["response_type"] = "code";
        query["client_id"] = _clientId;
        query["scope"] = scopes;
        query["redirect_uri"] = _redirectUri;
        query["state"] = state; // Important for CSRF protection and maintaining state across redirects

        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }

    /// <inheritdoc/>
    public async Task<AccessTokenResponse> ExchangeCodeForTokensAsync(string code)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", _redirectUri),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret)
        });

        // Ensure the HttpClient's base address is the TokenUrl for this request
        _httpClient.BaseAddress = new Uri(_tokenUrl);
        _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers to avoid conflicts
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.PostAsync("", content); // Post to the base address (TokenUrl)

        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new HttpRequestException("Failed to obtain user access token or token was empty.");
        }

        return tokenResponse;
    }

    /// <inheritdoc/>
    public void StoreUserTokens(AccessTokenResponse accessTokenResponse, string userId)
    {
        // In a real application, 'userId' would be a user's unique ID from your system
        // and tokens would be encrypted and stored persistently (e.g., database, secure cookie).
        _userTokensCache[userId] = accessTokenResponse;
    }

    /// <inheritdoc/>
    public AccessTokenResponse? GetUserTokens(string userId)
    {
        _userTokensCache.TryGetValue(userId, out var tokens);
        return tokens;
    }

    /// <inheritdoc/>
    public async Task<AccessTokenResponse> RefreshUserTokensAsync(string refreshToken)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret)
        });

        _httpClient.BaseAddress = new Uri(_tokenUrl);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.PostAsync("", content);

        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new HttpRequestException("Failed to refresh user access token or token was empty.");
        }

        return tokenResponse;
    }
}