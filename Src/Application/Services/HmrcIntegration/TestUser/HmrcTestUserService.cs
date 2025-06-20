using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Interfaces.Services.HmrcIntegration.Auth;
using Application.Interfaces.Services.HmrcIntegration.TestUser;
using Shared.Models.HmrcIntegration.TestUser;

// Required for ArgumentNullException

namespace Application.Services.HmrcIntegration.TestUser;

public class HmrcTestUserService : IHmrcTestUserService
{
    private readonly HttpClient _httpClient;
    private readonly IHmrcAuthService _hmrcAuthService;

    public HmrcTestUserService(HttpClient httpClient, IHmrcAuthService hmrcAuthService)
    {
        // Add null checks at the very beginning for all required dependencies
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
        if (hmrcAuthService == null) throw new ArgumentNullException(nameof(hmrcAuthService));

        _httpClient = httpClient;
        _hmrcAuthService = hmrcAuthService;
    }

    /// <inheritdoc/>
    public async Task<CreateIndividualTestUserResponse> CreateIndividualTestUserAsync(CreateIndividualTestUserRequest request)
    {
        // Get the access token
        var tokenResponse = await _hmrcAuthService.GetAccessTokenAsync();

        // Clear any existing Authorization header to avoid duplicates or stale tokens
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenResponse.TokenType, tokenResponse.AccessToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // Ensure Accept header is set for this client

        // The endpoint is relative to the base address configured for HttpClient
        var response = await _httpClient.PostAsJsonAsync("/create-test-user/individuals", request);

        response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP response status is an error code.

        var testUserResponse = await response.Content.ReadFromJsonAsync<CreateIndividualTestUserResponse>();

        // Modified check: Ensure the response object is not null AND a crucial property (MtdId) is present
        if (testUserResponse == null || string.IsNullOrEmpty(testUserResponse.MtdId))
        {
            throw new HttpRequestException("Failed to deserialize individual test user response or essential data (e.g., MtdId) is missing.");
        }

        return testUserResponse;
    }
}