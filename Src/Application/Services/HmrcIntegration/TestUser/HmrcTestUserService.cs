using Core.Interfaces.HmrcIntegration.Auth;
using Core.Interfaces.HmrcIntegration.TestUser;
using Core.Interfaces.Http;
using Core.Models.HmrcIntegration.TestUser;

namespace Application.Services.HmrcIntegration.TestUser;

public class HmrcTestUserService : IHmrcTestUserService
{
    private readonly IApiClient _hmrcClient;
    private readonly IHmrcAuthService _hmrcAuthService;
    private const string CreateIndividualTestUserEndpoint = "/create-test-user/individuals";

    public HmrcTestUserService(IApiClient hmrcClient, IHmrcAuthService hmrcAuthService)
    {
        _hmrcClient = hmrcClient ?? throw new ArgumentNullException(nameof(hmrcClient));
        _hmrcAuthService = hmrcAuthService ?? throw new ArgumentNullException(nameof(hmrcAuthService));
    }

    public async Task<CreateIndividualTestUserResponse> CreateIndividualTestUserAsync(CreateIndividualTestUserRequest request)
    {
        var tokenResponse = await _hmrcAuthService.GetAccessTokenAsync();

        var testUserResponse = await _hmrcClient.PostAsync<CreateIndividualTestUserRequest, CreateIndividualTestUserResponse>(
            CreateIndividualTestUserEndpoint,
            request,
            tokenResponse.AccessToken,
            tokenResponse.TokenType
        );

        // The application service retains the responsibility of validating the domain-specifics of the response.
        // The underlying client ensures the request was successful and deserialization worked.
        if (testUserResponse == null || string.IsNullOrEmpty(testUserResponse.MtdId))
        {
            // Using a more specific exception type is better. HttpRequestException implies a transport-level error,
            // but here the transport succeeded and the response content was invalid from our application's perspective.
            throw new InvalidOperationException("Received a valid response from HMRC, but it was missing essential data (e.g., MtdId).");
        }

        return testUserResponse;
    }
}