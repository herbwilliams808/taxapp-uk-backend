using System.Net.Http.Headers;
using Application.Interfaces.Services.HmrcIntegration.Auth;
using Application.Interfaces.Services.HmrcIntegration.TestUser;
using Application.Interfaces.Services.HmrcIntegration.Tutorial;

// Keep this for IHmrcAuthService and IHmrcUserAuthService
// For IHmrcTutorialService

// Required for ArgumentNullException

namespace Application.Services.HmrcIntegration.Tutorial;

public class HmrcTutorialService : IHmrcTutorialService
{
    private readonly HttpClient _httpClient;
    private readonly IHmrcAuthService _hmrcAuthService; // For application-restricted token
    private readonly IHmrcUserAuthService _hmrcUserAuthService; // For user-restricted token

    public HmrcTutorialService(
        HttpClient httpClient,
        IHmrcAuthService hmrcAuthService,
        IHmrcUserAuthService hmrcUserAuthService) // Inject new dependency
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
        if (hmrcAuthService == null) throw new ArgumentNullException(nameof(hmrcAuthService));
        if (hmrcUserAuthService == null) throw new ArgumentNullException(nameof(hmrcUserAuthService)); // Null check

        _httpClient = httpClient;
        _hmrcAuthService = hmrcAuthService;
        _hmrcUserAuthService = hmrcUserAuthService; // Assign new dependency
    }

    /// <inheritdoc/>
    public async Task<string> GetHelloWorldAsync()
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.hmrc.1.0+json"));

        var response = await _httpClient.GetAsync("/hello/world");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <inheritdoc/>
    public async Task<string> GetHelloApplicationAsync()
    {
        var tokenResponse = await _hmrcAuthService.GetAccessTokenAsync();

        // Clear headers before setting to avoid conflicts from previous calls
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenResponse.TokenType, tokenResponse.AccessToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.hmrc.1.0+json"));

        var response = await _httpClient.GetAsync("/hello/application");

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <inheritdoc/>
    public async Task<string> GetHelloUserAsync(string userId) // New method implementation
    {
        var userTokens = _hmrcUserAuthService.GetUserTokens(userId);
        if (userTokens == null)
        {
            throw new InvalidOperationException($"No user tokens found for userId: {userId}. Please authorize the user first.");
        }

        // Basic token expiration check (HMRC access tokens are usually short-lived, e.g., 4 hours)
        // For a real application, you'd want more robust expiration handling and proactive refreshing.
        if (userTokens.ExpiresIn < 60) // Refresh if less than 60 seconds left (or whatever threshold you prefer)
        {
            if (string.IsNullOrEmpty(userTokens.RefreshToken))
            {
                throw new InvalidOperationException($"Access token for user {userId} is expired and no refresh token available.");
            }
            // Attempt to refresh the token
            userTokens = await _hmrcUserAuthService.RefreshUserTokensAsync(userTokens.RefreshToken);
            _hmrcUserAuthService.StoreUserTokens(userTokens, userId); // Store the new tokens
        }

        // Clear headers before setting to avoid conflicts from previous calls
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(userTokens.TokenType, userTokens.AccessToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.hmrc.1.0+json"));

        var response = await _httpClient.GetAsync("/hello/user");

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}