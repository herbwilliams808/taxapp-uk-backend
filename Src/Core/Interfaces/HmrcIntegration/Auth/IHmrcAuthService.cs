

using Core.Models.HmrcIntegration.Auth;

namespace Core.Interfaces.HmrcIntegration.Auth;

public interface IHmrcAuthService
{
    Task<AccessTokenResponse> GetAccessTokenAsync();
}