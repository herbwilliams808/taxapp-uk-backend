using Shared.Models.Hmrc.Auth;

namespace Application.Interfaces.Services;

public interface IHmrcAuthService
{
    /// <summary>
    /// Retrieves an OAuth 2.0 access token for authenticating with HMRC APIs.
    /// </summary>
    /// <returns>An <see cref="AccessTokenResponse"/> containing the access token and its details.</returns>
    Task<AccessTokenResponse> GetAccessTokenAsync();
}