using System.Text.Json;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public class JsonDeserialiserService(ILogger<JsonDeserialiserService> logger) : IJsonDeserialiserService
{
    private readonly ILogger<JsonDeserialiserService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public T Deserialise<T>(string jsonContent)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new ArgumentException("JSON content cannot be null or empty.", nameof(jsonContent));
            }

            return JsonSerializer.Deserialize<T>(jsonContent)
                   ?? throw new InvalidOperationException("Failed to deserialize JSON content.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during JSON deserialization.");
            throw;
        }
    }

    public object? DeserialiseDynamic(string jsonContent)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new ArgumentException("JSON content cannot be null or empty.", nameof(jsonContent));
            }

            return JsonSerializer.Deserialize<JsonElement>(jsonContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during dynamic JSON deserialization.");
            throw;
        }
    }
}