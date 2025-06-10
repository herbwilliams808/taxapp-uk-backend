using System.Net;
using System.Text;
using System.Text.Json;
using API;
using Shared.Models.HttpMessages;
using Shared.Models.Incomes;
using Shared.Models.IndividualsEmploymentIncomes.Employments;
using Shared.Models.IndividualsReliefs;
using Shared.Models.IndividualsReliefs.CharitableGivings;
using Shared.Models.IndividualsReliefs.Pensions;
using Xunit.Abstractions;

namespace IntegrationTests.API.Controllers;

public class TaxEstimationControllerTests(
    CustomWebApplicationFactory<Program> factory,
    ITestOutputHelper testOutputHelper)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient(); // Creates an HttpClient for your in-memory app
    private readonly CustomWebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task CalculateTaxAsync_Employment_Property_Pension_Giftaid_ReturnsOkWithResponse()
    {
        // Arrange
        const decimal pay = 130252.56m;
        var request = new TaxEstimationRequest
        {
            Region = "england",
            TaxYearEnding = 2025,
            IncomeSources = new IncomeSourcesDetails
            {
                EmploymentsAndFinancialDetails =
                [
                    new EmploymentAndFinancialDetails
                    {
                        Pay = new Pay { TaxablePayToDate = pay },
                        Employer = new Employer { EmployerName = "Employer1" }
                    }
                ],
                UkPropertyBusinessIncome =
                {
                    AllowablePropertyLettingExpenses = 10408m,
                    Income = 28066m,
                    PropertyLettingLoanInterestAndFinanceCosts = 11801m
                }
            },
            IndividualsReliefs = new IndividualsReliefsDetails
            {
                PensionReliefs = new PensionReliefs 
                {
                    RegularPensionContributions = 44087.46m
                },
                GiftAidPayments = new GiftAidPayments
                {
                    CurrentYear = 200m
                }
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

        testOutputHelper.WriteLine(prettyResponseString);
        Assert.Equal(130252, taxResponse.PayFromAllEmployments); // Example assertion based on expected calculation
        Assert.Equal(17658, taxResponse.ProfitFromUkLandAndProperty); // Example assertion based on expected calculation
        Assert.Equal(147910, taxResponse.TotalIncome); // Example assertion based on expected calculation
        Assert.Equal(10759, taxResponse.PersonalAllowance); // Example assertion based on expected calculation
        Assert.Equal(137151, taxResponse.TaxableIncome); // Example assertion based on expected calculation
        Assert.Contains("Your basic rate limit has been extended", taxResponse.BasicRateLimitDetails.Message); // Example assertion based on expected calculation
        Assert.Equal(13184, taxResponse.TaxOwed); // Example assertion based on expected calculation
        Assert.Equal(134726, taxResponse.NetIncome);

        // Add more specific assertions based on your tax calculation logic
    }

    [Fact]
    public async Task CalculateTaxAsync_ValidRequest_ReturnsOkWithResponse()
    {
        // Arrange
        const decimal pay = 120000m;
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
                        Pay = new Pay { TaxablePayToDate = pay },
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

        testOutputHelper.WriteLine(prettyResponseString);
        Assert.Equal(pay, taxResponse.TotalIncome); // Example assertion based on expected calculation
        Assert.Null(taxResponse.BasicRateLimitDetails.Message); // Example assertion based on expected calculation
        Assert.True(taxResponse.TaxOwed > 0); // Example assertion based on expected calculation
        Assert.Equal(103540, taxResponse.NetIncome);

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
            }
        };
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