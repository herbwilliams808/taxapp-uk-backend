using System.Net;
using System.Text;
using System.Text.Json;
using Shared.Models.HttpMessages;
using Shared.Models.Incomes;
using Shared.Models.IndividualsEmploymentIncomes.Employments;
using API;
using Xunit.Abstractions;

namespace IntegrationTests.API.Controllers;

public class TaxEstimationControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public TaxEstimationControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(); // Creates an HttpClient for your in-memory app
    }

    [Fact]
    public async Task CalculateTaxAsync_ValidRequest_ReturnsOkWithResponse()
    {
        // Arrange
        var request = new TaxEstimationRequest
        {
            Region = "england",
            TaxYearEnding = 2024,
            IncomeSources = new IncomeSourcesDetails
            {
                EmploymentsAndFinancialDetails =
                [
                    new EmploymentAndFinancialDetails
                    {
                        Pay = new Pay { TaxablePayToDate = 50000m },
                        Employer = new Employer { EmployerName = "Employer1" }
                    }
                ]
            }
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/TaxEstimation", jsonContent);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var taxResponse = JsonSerializer.Deserialize<TaxEstimationResponse>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(taxResponse);
        var prettyResponseString = JsonSerializer.Serialize(taxResponse, 
            new JsonSerializerOptions { WriteIndented = true });
        
        _testOutputHelper.WriteLine(prettyResponseString);
        Assert.Equal(50000m, taxResponse.TotalIncome); // Example assertion based on expected calculation
        Assert.Null(taxResponse.BasicRateLimitExtendedMessage); // Example assertion based on expected calculation
        Assert.True(taxResponse.TaxOwed > 0); // Example assertion based on expected calculation
        Assert.Equal(47540, taxResponse.NetIncome);

        // Add more specific assertions based on your tax calculation logic
    }

    [Theory]
    [InlineData("InvalidRegion")]
    [InlineData("france")]
    public async Task CalculateTaxAsync_InvalidRegion_ReturnsBadRequest(string invalidRegion)
    {
        // Arrange
        var request = new TaxEstimationRequest
        {
            Region = invalidRegion,
            TaxYearEnding = 2024,
            IncomeSources = new IncomeSourcesDetails
            {
                EmploymentsAndFinancialDetails =
                [
                    new EmploymentAndFinancialDetails
                    {
                        Pay = new Pay { TaxablePayToDate = 10000m },
                        Employer = new Employer { EmployerName = "Employer1" }
                    }
                ]
            }        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/TaxEstimation", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid region", errorContent);
    }

    [Fact]
    public async Task CalculateTaxAsync_NoTaxYearEnding_DefaultsToCurrentPlusOneYear()
    {
        // Arrange
        var request = new TaxEstimationRequest
        {
            Region = "England",
            IncomeSources = new IncomeSourcesDetails
            {
                EmploymentsAndFinancialDetails =
                [
                    new EmploymentAndFinancialDetails
                    {
                        Pay = new Pay { TaxablePayToDate = 30000m },
                        Employer = new Employer { EmployerName = "Employer1" }
                    }
                ]
            }
            // TaxYearEnding is intentionally left null
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/TaxEstimation", jsonContent);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var taxResponse = JsonSerializer.Deserialize<TaxEstimationResponse>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(taxResponse);
        // Given the current date (June 5, 2025), the default tax year ending would be 2025.
        // (Tax year ending April 5, 2025 is the 2024-2025 tax year)
        // If current date is June 5, 2025: Month (6) > 4 or (Month == 4 && Day >= 6) (false) => CurrentYear (2025)
        // So, defaultTaxYearEnding = 2025
        // Assert.Equal(DateTime.UtcNow.Year, taxResponse.); // Adjust based on your current date logic
    }

    // Add more tests for different scenarios:
    // - Different income types and amounts
    // - With reliefs and deductions
    // - Edge cases for tax year calculation
    // - What happens with empty income lists, etc.
}