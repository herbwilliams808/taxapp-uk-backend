using Shared.Models.HmrcIntegration.Auth;

namespace Application.Interfaces.Services.HmrcIntegration.Auth;

public interface IHmrcUserAuthService
{
    /// <summary>
    /// Generates the HMRC authorization URL to which the user should be redirected.
    /// </summary>
    /// <param name="scopes">A space-delimited string of scopes to request (e.g., "hello").</param>
    /// <param name="state">An opaque value used to maintain state between the request and callback.</param>
    /// <returns>The URL to redirect the user to for HMRC authorization.</returns>
    string GetAuthorizationUrl(string scopes, string state);

    /// <summary>
    /// Exchanges an authorization code received from HMRC for access and refresh tokens.
    /// </summary>
    /// <param name="code">The authorization code received from HMRC.</param>
    /// <returns>An AccessTokenResponse containing the access token, refresh token, etc.</returns>
    Task<AccessTokenResponse> ExchangeCodeForTokensAsync(string code);

    /// <summary>
    /// Stores the user's access and refresh tokens temporarily for a session.
    /// In a real app, this would involve secure persistent storage.
    /// </summary>
    /// <param name="accessTokenResponse">The token response from HMRC.</param>
    /// <param name="userId">A unique identifier for the user (e.g., session ID).</param>
    void StoreUserTokens(AccessTokenResponse accessTokenResponse, string userId);

    /// <summary>
    /// Retrieves the user's access token from temporary storage.
    /// </summary>
    /// <param name="userId">The unique identifier for the user.</param>
    /// <returns>The stored AccessTokenResponse, or null if not found.</returns>
    AccessTokenResponse? GetUserTokens(string userId);

    /// <summary>
    /// Refreshes the user's access token using the refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <returns>A new AccessTokenResponse.</returns>
    Task<AccessTokenResponse> RefreshUserTokensAsync(string refreshToken);
}