using System.Text.Json;
using Application.Services.HmrcIntegration.Auth;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.TestHelpers;
// Assuming your service is in this namespace

namespace UnitTests.Application.Services;

public class TaxRatesCacheServiceTests
{
    private readonly Mock<ILogger<TaxRatesCacheService>> _mockLogger;
    private readonly TaxRatesCacheService _service;


    public TaxRatesCacheServiceTests()
    {
        _mockLogger = new Mock<ILogger<TaxRatesCacheService>>();
        _service = new TaxRatesCacheService(_mockLogger.Object);
    }

    // --- LoadCache Tests ---

    [Fact]
    public void LoadCache_ValidJson_LoadsSuccessfully()
    {
        // Arrange
        using var doc = JsonDocument.Parse(ValidTaxRates.ValidTaxRatesJson);
        var rootElement = doc.RootElement;

        // Act
        _service.LoadCache(rootElement);

        // Assert
        var cachedRates = _service.GetAllCachedRates();
        Assert.NotNull(cachedRates);
        Assert.Equal(2, cachedRates.Count); // year_2024_25 and year_2025_26
        Assert.True(cachedRates.ContainsKey("year_2024_25"));
        Assert.True(cachedRates.ContainsKey("year_2025_26"));
    }

    [Fact]
    public void LoadCache_InvalidRootJson_ThrowsArgumentException()
    {
        // Arrange - provide a JSON array instead of an object
        using var doc = JsonDocument.Parse("[]");
        var rootElement = doc.RootElement;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.LoadCache(rootElement));
        Assert.Contains("Provided taxRatesJson must be a JSON object.", ex.Message);
    }

    [Fact]
    public void LoadCache_MissingTaxRatesProperty_ThrowsArgumentException()
    {
        // Arrange - JSON without the "taxRates" root property
        using var doc = JsonDocument.Parse("{\"otherProperty\": {}}");
        var rootElement = doc.RootElement;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.LoadCache(rootElement));
        Assert.Contains("Expected a 'taxRates' property with a JSON object value at the root", ex.Message);
    }

    [Fact]
    public void LoadCache_TaxRatesPropertyNotObject_ThrowsArgumentException()
    {
        // Arrange - "taxRates" property is an array, not an object
        using var doc = JsonDocument.Parse("{\"taxRates\": []}");
        var rootElement = doc.RootElement;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.LoadCache(rootElement));
        Assert.Contains("Expected a 'taxRates' property with a JSON object value at the root", ex.Message);
    }

    [Fact]
    public void LoadCache_NonNumericLeafValue_ThrowsArgumentException()
    {
        // Arrange - introduce a string where a number is expected
        const string invalidJson = @"
            {
              ""taxRates"": {
                ""year_2024_25"": {
                  ""personalAllowance"": ""not_a_number""
                }
              }
            }";
        using var doc = JsonDocument.Parse(invalidJson);
        var rootElement = doc.RootElement;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.LoadCache(rootElement));
        Assert.Contains("Invalid value type at 'year_2024_25/personalAllowance': expected Number but found String.", ex.Message);
    }

    // --- GetTaxRateValue Tests ---

    [Fact]
    public void GetTaxRateValue_BeforeCacheLoaded_ThrowsInvalidOperationException()
    {
        // Arrange - service is initialized but LoadCache not called

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _service.GetTaxRateValue(2025, null, "personalAllowance"));
        Assert.Contains("Tax rates cache is not loaded.", ex.Message);
    }

    [Theory]
    [InlineData(2025, null, "personalAllowance", 12570)]
    [InlineData(2025, null, "basicRatePercentage", 20)]
    [InlineData(2025, null, "dividendBasicRatePercentage", 8.75D)] // Changed to double literal
    public void GetTaxRateValue_GeneralProperty_ReturnsCorrectValue(int year, string? region, string property, object expectedValue)
    {
        // Arrange
        LoadValidCache();

        // Act
        var actualValue = _service.GetTaxRateValue(year, region, property);

        // Assert
        Assert.NotNull(actualValue);
        // Fix: Compare as decimals or use specific Assert.Equal overload for numeric types if both are known at compile time.
        // Since actualValue can be int or decimal, and expectedValue is object,
        // we cast both to decimal for comparison to ensure type compatibility.
        Assert.Equal(Convert.ToDecimal(expectedValue), Convert.ToDecimal(actualValue));
    }

    [Theory]
    [InlineData(2025, "england", "basicRatePercentage", 20)]
    [InlineData(2025, "scotland", "starterRatePercentage", 19)]
    [InlineData(2025, "scotland", "topRatePercentage", 46)]
    [InlineData(2025, "northernireland", "higherRatePercentage", 40)]
    public void GetTaxRateValue_RegionalProperty_ReturnsCorrectValue(int year, string? region, string property, object expectedValue)
    {
        // Arrange
        LoadValidCache();

        // Act
        var actualValue = _service.GetTaxRateValue(year, region, property);

        // Assert
        Assert.NotNull(actualValue);
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void GetTaxRateValue_NonExistentYear_ReturnsNull()
    {
        // Arrange
        LoadValidCache();

        // Act
        var actualValue = _service.GetTaxRateValue(2030, null, "personalAllowance");

        // Assert
        Assert.Null(actualValue);
    }

    [Fact]
    public void GetTaxRateValue_NonExistentGeneralProperty_ReturnsNull()
    {
        // Arrange
        LoadValidCache();

        // Act
        var actualValue = _service.GetTaxRateValue(2025, null, "nonExistentProperty");

        // Assert
        Assert.Null(actualValue);
    }

    [Fact]
    public void GetTaxRateValue_NonExistentRegionalProperty_ReturnsNull()
    {
        // Arrange
        LoadValidCache();

        // Act
        var actualValue = _service.GetTaxRateValue(2025, "england", "nonExistentRegionalProperty");

        // Assert
        Assert.Null(actualValue);
    }

    [Fact]
    public void GetTaxRateValue_PropertyExistsOnlyRegionally_ReturnsNullIfRegionNotSpecified()
    {
        // Arrange
        LoadValidCache();

        // Act - "starterRatePercentage" exists only under "scotland"
        var actualValue = _service.GetTaxRateValue(2025, null, "starterRatePercentage");

        // Assert
        Assert.Null(actualValue); // Should be null because it's not a general property
    }

    [Fact]
    public void GetTaxRateValue_GeneralPropertyFoundEvenWhenRegionSpecified()
    {
        // Arrange
        LoadValidCache();

        // Act - personalAllowance is a general property, but we specify a region
        var actualValue = _service.GetTaxRateValue(2025, "england", "personalAllowance");

        // Assert
        Assert.NotNull(actualValue);
        Assert.Equal(12570, actualValue); // Should still find the general property
    }

    // --- GetDecimalTaxRateValue Tests ---

    [Theory]
    [InlineData(2025, null, "basicRatePercentage", 20.0)] // Integer value
    [InlineData(2025, null, "dividendBasicRatePercentage", 8.75)] // Decimal value
    [InlineData(2025, "scotland", "starterRatePercentage", 19.0)] // Regional integer value
    [InlineData(2025, "scotland", "intermediateRatePercentage", 21.0)] // Regional integer value
    public void GetDecimalTaxRateValue_ReturnsCorrectDecimal(int year, string? region, string property, decimal expectedValue)
    {
        // Arrange
        LoadValidCache();

        // Act
        var actualValue = _service.GetDecimalTaxRateValue(year, region, property);

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void GetDecimalTaxRateValue_NonExistentValue_ThrowsKeyNotFoundException()
    {
        // Arrange
        LoadValidCache();

        // Act & Assert
        var ex = Assert.Throws<KeyNotFoundException>(() => _service.GetDecimalTaxRateValue(2025, null, "nonExistentProperty"));
        Assert.Contains("Tax rate not found for year=2025, region='', property='nonExistentProperty'.", ex.Message);
    }

    // --- GetAllCachedRates Tests ---

    [Fact]
    public void GetAllCachedRates_BeforeCacheLoaded_ReturnsNull()
    {
        // Arrange - service is initialized but LoadCache not called

        // Act
        var cachedRates = _service.GetAllCachedRates();

        // Assert
        Assert.Null(cachedRates);
    }

    [Fact]
    public void GetAllCachedRates_AfterCacheLoaded_ReturnsLoadedCache()
    {
        // Arrange
        LoadValidCache();

        // Act
        var cachedRates = _service.GetAllCachedRates();

        // Assert
        Assert.NotNull(cachedRates);
        Assert.Equal(2, cachedRates.Count); // Based on ValidTaxRatesJson
    }

    // --- Helper Method ---
    private void LoadValidCache()
    {
        using var doc = JsonDocument.Parse(ValidTaxRates.ValidTaxRatesJson);
        _service.LoadCache(doc.RootElement);
    }
}