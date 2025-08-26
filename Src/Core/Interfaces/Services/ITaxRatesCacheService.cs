using System.Text.Json;

// For JsonElement

namespace Core.Interfaces.Services;

public interface ITaxRatesCacheService
{
    void LoadCache(JsonElement taxRatesJson);

    object? GetTaxRateValue(int year, string? region, string property);

    decimal GetDecimalTaxRateValue(int year, string? region, string property);

    Dictionary<string, JsonElement>? GetAllCachedRates();
}