using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API;
using Application.Interfaces.Services;
using Moq;
using Shared.Models.Hmrc.TestUser;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection; // Required for WithWebHostBuilder and ConfigureServices

namespace IntegrationTests.API.Controllers;

public class HmrcTestUserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public HmrcTestUserControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateIndividualTestUser_ValidRequest_ReturnsOkWithTestUserDetails()
    {
        // Arrange
        var mockMtdId = "XAMTD99999999999";
        var mockGatewayId = "gatewayXYZ";
        var mockPassword = "passwordABC";
        var expectedUserFullName = "Integration Test User";
        var expectedEmailAddress = "integration.test@example.com";

        // Mock the IHmrcTestUserService to control its behavior
        var mockHmrcTestUserService = new Mock<IHmrcTestUserService>();
        mockHmrcTestUserService.Setup(s => s.CreateIndividualTestUserAsync(It.IsAny<CreateIndividualTestUserRequest>()))
            .ReturnsAsync(new CreateIndividualTestUserResponse
            {
                MtdId = mockMtdId,
                GatewayId = mockGatewayId,
                Password = mockPassword,
                UserFullName = expectedUserFullName,
                EmailAddress = expectedEmailAddress
            });

        // Create a new HttpClient for this test, overriding the service registration
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing registration of IHmrcTestUserService
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHmrcTestUserService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                // Add your mock implementation
                services.AddScoped<IHmrcTestUserService>(_ => mockHmrcTestUserService.Object);
            });
        }).CreateClient();


        var request = new CreateIndividualTestUserRequest
        {
            ServiceNames = new List<string> { "mtd-income-tax", "mtd-vat" }
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/hmrc/test-users/individual", jsonContent);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var testUserResponse = JsonSerializer.Deserialize<CreateIndividualTestUserResponse>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(testUserResponse);
        Assert.Equal(mockMtdId, testUserResponse.MtdId);
        Assert.Equal(mockGatewayId, testUserResponse.GatewayId);
        Assert.Equal(mockPassword, testUserResponse.Password);
        Assert.Equal(expectedUserFullName, testUserResponse.UserFullName);
        Assert.Equal(expectedEmailAddress, testUserResponse.EmailAddress);

        mockHmrcTestUserService.Verify(s => s.CreateIndividualTestUserAsync(
            It.Is<CreateIndividualTestUserRequest>(r =>
                r.ServiceNames.Contains("mtd-income-tax") &&
                r.ServiceNames.Contains("mtd-vat") &&
                r.ServiceNames.Count == 2
            )), Times.Once());
    }

    [Fact]
    public async Task CreateIndividualTestUser_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        // An empty request for service names is valid, so let's send a malformed JSON or invalid data
        var malformedJsonContent = new StringContent("{ \"ServiceNames\": [123] }", Encoding.UTF8, "application/json");

        var client = _factory.CreateClient(); // Use default client, no need to mock service if request is invalid before service call

        // Act
        var response = await client.PostAsync("/api/hmrc/test-users/individual", malformedJsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(errorContent);
        Assert.Contains("The JSON value could not be converted to System.String.", errorContent); // Example error from model binding
    }

    // Add more integration tests for scenarios like:
    // - Service throwing an exception (e.g., HttpRequestException from HMRC API)
    // - Different combinations of service names
}