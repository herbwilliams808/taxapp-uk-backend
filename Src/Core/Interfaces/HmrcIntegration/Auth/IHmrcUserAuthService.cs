using Core.Models.HmrcIntegration.Auth;

namespace Core.Interfaces.HmrcIntegration.Auth;

public interface IHmrcUserAuthService
{
    string GetAuthorizationUrl(string scopes, string state);

    Task<AccessTokenResponse> ExchangeCodeForTokensAsync(string code);

    void StoreUserTokens(AccessTokenResponse accessTokenResponse, string userId);

    AccessTokenResponse? GetUserTokens(string userId);

    Task<AccessTokenResponse> RefreshUserTokensAsync(string refreshToken);
}