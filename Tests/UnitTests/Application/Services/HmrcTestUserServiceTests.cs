using Application.Services.HmrcIntegration.TestUser;
using Core.Interfaces.HmrcIntegration.Auth;
using Core.Interfaces.Http;
using Core.Models.HmrcIntegration.Auth;
using Core.Models.HmrcIntegration.TestUser;
using Moq;

namespace UnitTests.Application.Services;

public class HmrcTestUserServiceTests
{
    private readonly Mock<IApiClient> _mockHmrcClient;
    private readonly Mock<IHmrcAuthService> _mockAuthService;
    private readonly HmrcTestUserService _sut;

    public HmrcTestUserServiceTests()
    {
        _mockHmrcClient = new Mock<IApiClient>();
        _mockAuthService = new Mock<IHmrcAuthService>();
        _sut = new HmrcTestUserService(_mockHmrcClient.Object, _mockAuthService.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_InitializesService()
    {
        // Arrange & Act
        // Assert
        Assert.NotNull(_sut);
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
        Assert.Throws<ArgumentNullException>(() => new HmrcTestUserService(_mockHmrcClient.Object, null!));
    }

    #endregion

    #region CreateIndividualTestUserAsync Tests

    [Fact]
    public async Task CreateIndividualTestUserAsync_ReturnsUserSuccessfully()
    {
        // Arrange
        var request = new CreateIndividualTestUserRequest { ServiceNames = { "mtd-income-tax" } };
        var tokenResponse = new AccessTokenResponse { AccessToken = "mock-token", TokenType = "Bearer" };
        var expectedResponse = new CreateIndividualTestUserResponse
        {
            MtdId = "XAMTD00000000001",
            GatewayId = "gateway123",
            Password = "password"
        };

        _mockAuthService.Setup(s => s.GetAccessTokenAsync())
            .ReturnsAsync(tokenResponse);

        _mockHmrcClient.Setup(c => c.PostAsync<CreateIndividualTestUserRequest, CreateIndividualTestUserResponse>(
                It.IsAny<string>(),
                request,
                tokenResponse.AccessToken,
                tokenResponse.TokenType))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _sut.CreateIndividualTestUserAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.MtdId, result.MtdId);
        Assert.Equal(expectedResponse.GatewayId, result.GatewayId);
        Assert.Equal(expectedResponse.Password, result.Password);

        _mockAuthService.Verify(s => s.GetAccessTokenAsync(), Times.Once());
        _mockHmrcClient.Verify(c => c.PostAsync<CreateIndividualTestUserRequest, CreateIndividualTestUserResponse>(
            "/create-test-user/individuals",
            request,
            tokenResponse.AccessToken,
            tokenResponse.TokenType),
            Times.Once(),
            "The PostAsync method was not called with the expected parameters.");
    }

    [Theory]
    [InlineData("Simulated bad request")]
    [InlineData("Simulated server error")]
    public async Task CreateIndividualTestUserAsync_PropagatesException_WhenClientThrows(string exceptionMessage)
    {
        // Arrange
        var request = new CreateIndividualTestUserRequest();
        var tokenResponse = new AccessTokenResponse { AccessToken = "mock-token", TokenType = "Bearer" };
        _mockAuthService.Setup(s => s.GetAccessTokenAsync())
            .ReturnsAsync(tokenResponse);

        _mockHmrcClient.Setup(c => c.PostAsync<CreateIndividualTestUserRequest, CreateIndividualTestUserResponse>(
                It.IsAny<string>(), It.IsAny<CreateIndividualTestUserRequest>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException(exceptionMessage));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<HttpRequestException>(() => _sut.CreateIndividualTestUserAsync(request));
        Assert.Equal(exceptionMessage, ex.Message);
    }

    [Fact]
    public async Task CreateIndividualTestUserAsync_ThrowsInvalidOperationException_WhenResponseIsMissingMtdId()
    {
        // Arrange
        var request = new CreateIndividualTestUserRequest();
        var tokenResponse = new AccessTokenResponse { AccessToken = "mock-token", TokenType = "Bearer" };
        var invalidResponse = new CreateIndividualTestUserResponse { MtdId = null }; // MtdId is missing

        _mockAuthService.Setup(s => s.GetAccessTokenAsync())
            .ReturnsAsync(tokenResponse);

        _mockHmrcClient.Setup(c => c.PostAsync<CreateIndividualTestUserRequest, CreateIndividualTestUserResponse>(
                It.IsAny<string>(), It.IsAny<CreateIndividualTestUserRequest>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(invalidResponse);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateIndividualTestUserAsync(request));
        Assert.Equal("Received a valid response from HMRC, but it was missing essential data (e.g., MtdId).", ex.Message);
    }

    [Fact]
    public async Task CreateIndividualTestUserAsync_ThrowsInvalidOperationException_WhenResponseIsNull()
    {
        // Arrange
        var request = new CreateIndividualTestUserRequest();
        var tokenResponse = new AccessTokenResponse { AccessToken = "mock-token", TokenType = "Bearer" };

        _mockAuthService.Setup(s => s.GetAccessTokenAsync())
            .ReturnsAsync(tokenResponse);

        _mockHmrcClient.Setup(c => c.PostAsync<CreateIndividualTestUserRequest, CreateIndividualTestUserResponse>(
                It.IsAny<string>(), It.IsAny<CreateIndividualTestUserRequest>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((CreateIndividualTestUserResponse)null!);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateIndividualTestUserAsync(request));
        Assert.Equal("Received a valid response from HMRC, but it was missing essential data (e.g., MtdId).", ex.Message);
    }

    #endregion
}