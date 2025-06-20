using System.Text.Json; // For JsonElement

namespace Application.Interfaces.Services.HmrcIntegration.TestUser;

public interface ITaxRatesCacheService
{
    /// <summary>
    /// Loads tax rates JSON data into the cache.
    /// Validates all leaf values are numbers.
    /// </summary>
    /// <param name="taxRatesJson">Root JSON element containing the tax rates object.</param>
    /// <exception cref="ArgumentException">Thrown if input JSON is invalid.</exception>
    void LoadCache(JsonElement taxRatesJson);

    /// <summary>
    /// Retrieve a tax rate value by tax year, region (optional), and property.
    /// </summary>
    /// <returns>An object representing the tax rate value (int or decimal), or null if not found/invalid.</returns>
    object? GetTaxRateValue(int year, string? region, string property);

    /// <summary>
    /// Retrieve a decimal tax rate value by tax year, region (optional), and property.
    /// Throws if value not found or not convertible.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown if the tax rate is not found.</exception>
    /// <exception cref="InvalidCastException">Thrown if the found value cannot be converted to decimal.</exception>
    decimal GetDecimalTaxRateValue(int year, string? region, string property);

    /// <summary>
    /// Returns all cached tax rates as a dictionary of tax year keys to deserialized objects.
    /// Returns null if cache is not loaded.
    /// </summary>
    Dictionary<string, JsonElement>? GetAllCachedRates();
}