using Core.Interfaces.HmrcIntegration.Auth;
using Core.Interfaces.HmrcIntegration.Tutorial;
using Core.Interfaces.Http;

namespace Application.Services.HmrcIntegration.Tutorial;

public class HmrcTutorialService : IHmrcTutorialService
{
    private readonly IApiClient _hmrcClient;
    private readonly IHmrcAuthService _hmrcAuthService; // For application-restricted token
    private readonly IHmrcUserAuthService _hmrcUserAuthService; // For user-restricted token

    public HmrcTutorialService(
        IApiClient hmrcClient,
        IHmrcAuthService hmrcAuthService,
        IHmrcUserAuthService hmrcUserAuthService)
    {
        _hmrcClient = hmrcClient ?? throw new ArgumentNullException(nameof(hmrcClient));
        _hmrcAuthService = hmrcAuthService ?? throw new ArgumentNullException(nameof(hmrcAuthService));
        _hmrcUserAuthService = hmrcUserAuthService ?? throw new ArgumentNullException(nameof(hmrcUserAuthService));
    }

    public async Task<string> GetHelloWorldAsync()
    {
        return await _hmrcClient.GetAsync("/hello/world");
    }

    public async Task<string> GetHelloApplicationAsync()
    {
        var tokenResponse = await _hmrcAuthService.GetAccessTokenAsync();
        return await _hmrcClient.GetWithAuthAsync("/hello/application", tokenResponse.AccessToken, tokenResponse.TokenType);
    }

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

        return await _hmrcClient.GetWithAuthAsync("/hello/user", userTokens.AccessToken, userTokens.TokenType);
    }
}