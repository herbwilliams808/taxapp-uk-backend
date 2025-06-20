using Moq;
using Xunit;
using Application.Services.HmrcIntegration.Auth;
using System.Net.Http;
using System.Threading;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Application.Interfaces.Services.HmrcIntegration.Auth;
using Application.Interfaces.Services.HmrcIntegration.TestUser;
using Application.Services.HmrcIntegration.TestUser;
using Moq.Protected;
using Shared.Models.HmrcIntegration.Auth;
using Shared.Models.HmrcIntegration.TestUser; // Added this using directive

namespace UnitTests.Application.Services;

public class HmrcTestUserServiceTests
{
    private readonly Mock<HttpClient> _mockHttpClient;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHmrcAuthService> _mockAuthService;

    public HmrcTestUserServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClient = new Mock<HttpClient>(_mockHttpMessageHandler.Object)
        {
            CallBase = true // Ensures real methods are called on HttpClient where not mocked
        };
        _mockHttpClient.Object.BaseAddress = new Uri("https://test-api.service.hmrc.gov.uk/");

        _mockAuthService = new Mock<IHmrcAuthService>();
    }

    private HttpResponseMessage CreateTestUserHttpResponse(string mtdId, string gatewayId, string password)
    {
        var responseContent = new CreateIndividualTestUserResponse
        {
            MtdId = mtdId,
            GatewayId = gatewayId,
            Password = password,
            UserFullName = "Test User",
            EmailAddress = "test@example.com"
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
        var service = new HmrcTestUserService(_mockHttpClient.Object, _mockAuthService.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HmrcTestUserService(null!, _mockAuthService.Object));
    }

    [Fact]
    public void Constructor_WithNullAuthService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HmrcTestUserService(_mockHttpClient.Object, null!));
    }

    #endregion

    #region CreateIndividualTestUserAsync Tests

    [Fact]
    public async Task CreateIndividualTestUserAsync_ReturnsUserSuccessfully()
    {
        // Arrange
        var accessToken = "mockBearerToken123";
        var expectedMtdId = "XAMTD00000000001";
        var expectedGatewayId = "gateway123";
        var expectedPassword = "password";

        _mockAuthService.Setup(s => s.GetAccessTokenAsync())
            .ReturnsAsync(new AccessTokenResponse { AccessToken = accessToken, TokenType = "Bearer", ExpiresIn = 3600 });

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.ToString().EndsWith("/create-test-user/individuals") &&
                    req.Method == HttpMethod.Post &&
                    req.Headers.Authorization!.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == accessToken),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(CreateTestUserHttpResponse(expectedMtdId, expectedGatewayId, expectedPassword));

        var service = new HmrcTestUserService(_mockHttpClient.Object, _mockAuthService.Object);
        var request = new CreateIndividualTestUserRequest { ServiceNames = { "mtd-income-tax" } };

        // Act
        var result = await service.CreateIndividualTestUserAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedMtdId, result.MtdId);
        Assert.Equal(expectedGatewayId, result.GatewayId);
        Assert.Equal(expectedPassword, result.Password);

        _mockAuthService.Verify(s => s.GetAccessTokenAsync(), Times.Once());
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task CreateIndividualTestUserAsync_ThrowsHttpRequestException_OnUnsuccessfulStatusCode(HttpStatusCode statusCode)
    {
        // Arrange
        var accessToken = "mockBearerToken";
        _mockAuthService.Setup(s => s.GetAccessTokenAsync())
            .ReturnsAsync(new AccessTokenResponse { AccessToken = accessToken, TokenType = "Bearer", ExpiresIn = 3600 });

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(statusCode));

        var service = new HmrcTestUserService(_mockHttpClient.Object, _mockAuthService.Object);
        var request = new CreateIndividualTestUserRequest();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<HttpRequestException>(() => service.CreateIndividualTestUserAsync(request));
        Assert.Contains(((int)statusCode).ToString(), ex.Message);
    }

    [Fact]
    public async Task CreateIndividualTestUserAsync_ThrowsHttpRequestException_WhenResponseIsEmpty()
    {
        // Arrange
        var accessToken = "mockBearerToken";
        _mockAuthService.Setup(s => s.GetAccessTokenAsync())
            .ReturnsAsync(new AccessTokenResponse { AccessToken = accessToken, TokenType = "Bearer", ExpiresIn = 3600 });

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

        var service = new HmrcTestUserService(_mockHttpClient.Object, _mockAuthService.Object);
        var request = new CreateIndividualTestUserRequest();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<HttpRequestException>(() => service.CreateIndividualTestUserAsync(request));
        // Updated assertion to match the new exception message
        Assert.Contains("Failed to deserialize individual test user response or essential data (e.g., MtdId) is missing.", ex.Message);
    }

    #endregion
}