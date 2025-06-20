using Shared.Models.HmrcIntegration.Auth;

namespace Application.Interfaces.Services.HmrcIntegration.Auth;

public interface IHmrcAuthService
{
    /// <summary>
    /// Retrieves an OAuth 2.0 access token for authenticating with HMRC APIs.
    /// </summary>
    /// <returns>An <see cref="AccessTokenResponse"/> containing the access token and its details.</returns>
    Task<AccessTokenResponse> GetAccessTokenAsync();
}