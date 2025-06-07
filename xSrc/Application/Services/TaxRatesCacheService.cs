using System.Text.Json;
using Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class TaxRatesCacheService: ITaxRatesCacheService
{
    private readonly ILogger<TaxRatesCacheService> _logger;

    // Cache dictionary: taxYearKey -> JSON object
    // Changed type from 'object' to 'JsonElement' to correctly handle deserialization with System.Text.Json
    private Dictionary<string, JsonElement>? _cachedTaxRates;

    public TaxRatesCacheService(ILogger<TaxRatesCacheService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Loads tax rates JSON data into the cache.
    /// Validates all leaf values are numbers.
    /// </summary>
    /// <param name="taxRatesJson">Root JSON element containing the tax rates object.</param>
    public void LoadCache(JsonElement taxRatesJson)
    {
        if (taxRatesJson.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("Provided taxRatesJson must be a JSON object.", nameof(taxRatesJson));
        }

        if (!taxRatesJson.TryGetProperty("taxRates", out var taxRatesElement) || taxRatesElement.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("Expected a 'taxRates' property with a JSON object value at the root of the provided JSON.", nameof(taxRatesJson));
        }

        var tempCache = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);

        foreach (var property in taxRatesElement.EnumerateObject())
        {
            ValidateAllValuesAreNumbers(property.Value, property.Name);
            // CRITICAL FIX: Clone the JsonElement to prevent ObjectDisposedException.
            // JsonElement refers to a JsonDocument, which might be disposed elsewhere.
            // Cloning creates an independent copy with its own underlying JsonDocument.
            tempCache[property.Name] = property.Value.Clone();
        }

        _cachedTaxRates = tempCache;

        _logger.LogInformation("Tax rates cache loaded with {Count} tax year entries.", _cachedTaxRates.Count);

        if (_cachedTaxRates != null)
        {
            // For logging, you might want to serialize it for readability, but for cache itself, JsonElement is better
            // string cachedTaxRatesJson = JsonSerializer.Serialize(_cachedTaxRates, new JsonSerializerOptions
            // {
            //     WriteIndented = true // Makes the output more human-readable
            // });
            // _logger.LogInformation("Cached tax rates:\n {x}", cachedTaxRatesJson);
        }
        else
        {
            _logger.LogWarning("Tax rates cache is empty.");
        }
    }

    /// <summary>
    /// Recursively validates that all leaf values in the JSON element are numbers.
    /// Throws ArgumentException if a non-number leaf is found.
    /// </summary>
    private void ValidateAllValuesAreNumbers(JsonElement element, string path = "")
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in element.EnumerateObject())
                {
                    ValidateAllValuesAreNumbers(prop.Value, $"{path}/{prop.Name}");
                }
                break;
            case JsonValueKind.Array:
                int i = 0;
                foreach (var item in element.EnumerateArray())
                {
                    ValidateAllValuesAreNumbers(item, $"{path}[{i}]");
                    i++;
                }
                break;
            case JsonValueKind.Number:
                // OK
                break;
            default:
                throw new ArgumentException($"Invalid value type at '{path}': expected Number but found {element.ValueKind}.");
        }
    }

    /// <summary>
    /// Retrieve a tax rate value by tax year, region (optional), and property.
    /// </summary>
    public object? GetTaxRateValue(int year, string? region, string property)
    {
        if (_cachedTaxRates == null)
        {
            _logger.LogWarning("Attempted to get tax rate before cache was loaded.");
            throw new InvalidOperationException("Tax rates cache is not loaded.");
        }

        var taxYearKey = $"year_{year - 1}_{year - 2000}";

        if (!_cachedTaxRates.TryGetValue(taxYearKey, out JsonElement yearElement))
        {
            _logger.LogWarning("Tax year '{TaxYear}' not found in cache.", taxYearKey);
            return null;
        }

        if (yearElement.ValueKind != JsonValueKind.Object)
        {
            _logger.LogWarning("Invalid format for tax year '{TaxYear}'. Expected a JSON object.", taxYearKey);
            return null;
        }

        // Attempt to get regional property first if a region is specified
        if (!string.IsNullOrWhiteSpace(region))
        {
            if (yearElement.TryGetProperty(region.ToLowerInvariant(), out JsonElement regionElement))
            {
                if (regionElement.ValueKind == JsonValueKind.Object)
                {
                    if (regionElement.TryGetProperty(property, out JsonElement regionalPropertyValue))
                    {
                        if (regionalPropertyValue.ValueKind == JsonValueKind.Number)
                        {
                            // Return as int or decimal based on the number type
                            if (regionalPropertyValue.TryGetInt32(out int intValue)) return intValue;
                            if (regionalPropertyValue.TryGetDecimal(out decimal decimalValue)) return decimalValue;
                        }
                        _logger.LogWarning("Regional property '{Property}' under region '{Region}' for tax year '{TaxYear}' is not a number. Found {ValueKind}.", property, region, taxYearKey, regionalPropertyValue.ValueKind);
                        return null; // Value found but not a number
                    }
                }
                else
                {
                    _logger.LogWarning("Region '{Region}' for tax year '{TaxYear}' is not a JSON object. Found {ValueKind}.", region, taxYearKey, regionElement.ValueKind);
                }
            }
        }

        // Fallback to general property directly under the tax year if regional property was not found
        // or if no region was specified.
        if (yearElement.TryGetProperty(property, out JsonElement generalPropertyValue))
        {
            if (generalPropertyValue.ValueKind == JsonValueKind.Number)
            {
                // Return as int or decimal based on the number type
                if (generalPropertyValue.TryGetInt32(out int intValue)) return intValue;
                if (generalPropertyValue.TryGetDecimal(out decimal decimalValue)) return decimalValue;
            }
            _logger.LogWarning("General property '{Property}' for tax year '{TaxYear}' is not a number. Found {ValueKind}.", property, taxYearKey, generalPropertyValue.ValueKind);
            return null; // Value found but not a number
        }

        _logger.LogWarning("Property '{Property}' not found for tax year '{TaxYear}' (and no regional property found for '{Region}').", property, taxYearKey, region);
        return null;
    }

    /// <summary>
    /// Retrieve a decimal tax rate value by tax year, region (optional), and property.
    /// Throws if value not found or not convertible.
    /// </summary>
    public decimal GetDecimalTaxRateValue(int year, string? region, string property)
    {
        var valueObj = GetTaxRateValue(year, region, property);

        if (valueObj == null)
            throw new KeyNotFoundException($"Tax rate not found for year={year}, region='{region}', property='{property}'.");

        if (valueObj is int i) // Handle int directly, it's convertible to decimal
            return i;

        if (valueObj is decimal d)
            return d;

        if (decimal.TryParse(valueObj.ToString(), out var result))
            return result;

        throw new InvalidCastException($"Tax rate value '{valueObj}' could not be converted to decimal.");
    }

    /// <summary>
    /// Returns all cached tax rates as a dictionary of tax year keys to deserialized objects.
    /// Returns null if cache is not loaded.
    /// </summary>
    // Changed return type to JsonElement to reflect the actual cached type
    public Dictionary<string, JsonElement>? GetAllCachedRates()
    {
        return _cachedTaxRates;
    }
}