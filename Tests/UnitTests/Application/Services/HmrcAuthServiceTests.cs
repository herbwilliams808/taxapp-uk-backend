using System.Net;
using System.Text;
using System.Text.Json;
using Application.Services.HmrcIntegration.Auth;
using Core.Models.HmrcIntegration.Auth;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

// For Stopwatch

namespace UnitTests.Application.Services;

public class HmrcAuthServiceTests
{
    private readonly Mock<HttpClient> _mockHttpClient;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly IConfiguration _configuration;

    public HmrcAuthServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClient = new Mock<HttpClient>(_mockHttpMessageHandler.Object)
        {
            CallBase = true // Ensures real methods are called on HttpClient where not mocked
        };
        _mockHttpClient.Object.BaseAddress = new Uri("https://test-api.service.hmrc.gov.uk/oauth/token");

        var inMemorySettings = new Dictionary<string, string?>
        {
            {"HMRC_TestApi:ClientId", "testClientId"},
            {"HMRC_TestApi:ClientSecret", "testClientSecret"},
            {"HMRC_TestApi:TokenUrl", "https://test-api.service.hmrc.gov.uk/oauth/token"} // Explicitly set token URL
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    private HttpResponseMessage CreateTokenHttpResponse(string accessToken, int expiresIn, string tokenType = "Bearer")
    {
        var responseContent = new AccessTokenResponse
        {
            AccessToken = accessToken,
            TokenType = tokenType,
            ExpiresIn = expiresIn
        };
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(responseContent), Encoding.UTF8, "application/json")
        };
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_InitializesService()
    {
        // Arrange & Act
        var service = new HmrcAuthService(_mockHttpClient.Object, _configuration);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HmrcAuthService(null!, _configuration));
    }

    [Fact]
    public void Constructor_WithNullConfiguration_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HmrcAuthService(_mockHttpClient.Object, null!));
    }

    [Fact]
    public void Constructor_MissingClientId_ThrowsInvalidOperationException()
    {
        // Arrange
        var missingClientIdConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"HMRC_TestApi:ClientSecret", "testClientSecret"},
                {"HMRC_TestApi:TokenUrl", "https://test-api.service.hmrc.gov.uk/oauth/token"}
            })
            .Build();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => new HmrcAuthService(_mockHttpClient.Object, missingClientIdConfig));
        Assert.Contains("HMRC_TestApi:ClientId is not configured.", ex.Message);
    }

    [Fact]
    public void Constructor_MissingClientSecret_ThrowsInvalidOperationException()
    {
        // Arrange
        var missingClientSecretConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"HMRC_TestApi:ClientId", "testClientId"},
                {"HMRC_TestApi:TokenUrl", "https://test-api.service.hmrc.gov.uk/oauth/token"}
            })
            .Build();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => new HmrcAuthService(_mockHttpClient.Object, missingClientSecretConfig));
        Assert.Contains("HMRC_TestApi:ClientSecret is not configured.", ex.Message);
    }

    #endregion

    #region GetAccessTokenAsync Tests

    [Fact]
    public async Task GetAccessTokenAsync_ReturnsTokenSuccessfully()
    {
        // Arrange
        var expectedToken = "mockAccessToken123";
        var expiresIn = 3600;
        _mockHttpMessageHandler.Protected() // Use Protected() to mock SendAsync
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(CreateTokenHttpResponse(expectedToken, expiresIn));

        var service = new HmrcAuthService(_mockHttpClient.Object, _configuration);

        // Act
        var result = await service.GetAccessTokenAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.AccessToken);
        Assert.Equal(expiresIn, result.ExpiresIn);
        Assert.Equal("Bearer", result.TokenType);

        // Verify that SendAsync was called exactly once
        _mockHttpMessageHandler.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task GetAccessTokenAsync_CachesToken_ForSubsequentCalls()
    {
        // Arrange
        var firstToken = "mockAccessToken1";
        var secondToken = "mockAccessToken2"; // This should not be used
        var expiresIn = 300; // 5 minutes validity

        // First call will return firstToken
        _mockHttpMessageHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(CreateTokenHttpResponse(firstToken, expiresIn))
            .ReturnsAsync(CreateTokenHttpResponse(secondToken, expiresIn)); // Should not be called by the second GetAccessTokenAsync

        var service = new HmrcAuthService(_mockHttpClient.Object, _configuration);

        // Act - First call to get token
        var result1 = await service.GetAccessTokenAsync();
        Assert.Equal(firstToken, result1.AccessToken);

        // Act - Second call, should use cached token
        var result2 = await service.GetAccessTokenAsync();
        Assert.Equal(firstToken, result2.AccessToken); // Should be the same as firstToken

        // Assert - Verify SendAsync was called only once for two GetAccessTokenAsync calls
        _mockHttpMessageHandler.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task GetAccessTokenAsync_RefreshesToken_WhenExpired()
    {
        // Arrange
        var initialToken = "initialToken";
        var refreshedToken = "refreshedToken";
        var shortExpiry = 1; // Token expires in 1 second

        _mockHttpMessageHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(CreateTokenHttpResponse(initialToken, shortExpiry)) // Token for first call
            .ReturnsAsync(CreateTokenHttpResponse(refreshedToken, 3600)); // New token for second call after expiry

        var service = new HmrcAuthService(_mockHttpClient.Object, _configuration);

        // Act - First call to get token (will be cached for 1s - 60s buffer = immediately expired)
        var result1 = await service.GetAccessTokenAsync();
        Assert.Equal(initialToken, result1.AccessToken);

        // Wait for token to expire (simulated by the shortExpiry)
        await Task.Delay(TimeSpan.FromSeconds(shortExpiry + 0.1)); // Wait slightly more than expiry time

        // Act - Second call, should trigger refresh
        var result2 = await service.GetAccessTokenAsync();
        Assert.Equal(refreshedToken, result2.AccessToken); // Should be the new token

        // Assert - Verify SendAsync was called twice (initial + refresh)
        _mockHttpMessageHandler.Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetAccessTokenAsync_ThrowsHttpRequestException_OnUnsuccessfulStatusCode(HttpStatusCode statusCode)
    {
        // Arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(statusCode));

        var service = new HmrcAuthService(_mockHttpClient.Object, _configuration);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAccessTokenAsync());
        Assert.Contains(((int)statusCode).ToString(), ex.Message); // Check for status code in message
    }

    [Fact]
    public async Task GetAccessTokenAsync_ThrowsHttpRequestException_WhenTokenResponseIsEmpty()
    {
        // Arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json") // Empty JSON
            });

        var service = new HmrcAuthService(_mockHttpClient.Object, _configuration);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAccessTokenAsync());
        Assert.Contains("Failed to obtain access token or token was empty.", ex.Message);
    }

    [Fact]
    public async Task GetAccessTokenAsync_ThrowsHttpRequestException_WhenAccessTokenIsNull()
    {
        // Arrange
        var responseContent = new AccessTokenResponse
        {
            AccessToken = null!, // Explicitly null
            TokenType = "Bearer",
            ExpiresIn = 3600
        };
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseContent), Encoding.UTF8, "application/json")
            });

        var service = new HmrcAuthService(_mockHttpClient.Object, _configuration);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAccessTokenAsync());
        Assert.Contains("Failed to obtain access token or token was empty.", ex.Message);
    }

    #endregion
}