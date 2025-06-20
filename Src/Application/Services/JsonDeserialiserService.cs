using System.Text.Json;
using Application.Interfaces.Services.HmrcIntegration.TestUser;
using Microsoft.Extensions.Logging;

namespace Application.Services.HmrcIntegration.Auth;

public class JsonDeserialiserService(ILogger<JsonDeserialiserService> logger) : IJsonDeserialiserService
{
    private readonly ILogger<JsonDeserialiserService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Deserializes JSON content into a strongly typed object.
    /// </summary>
    /// <typeparam name="T">The type to which the JSON should be deserialized.</typeparam>
    /// <param name="jsonContent">The JSON string to deserialize.</param>
    /// <returns>The deserialized object of type T.</returns>
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

    /// <summary>
    /// Deserializes JSON content into a dynamic object.
    /// </summary>
    /// <param name="jsonContent">The JSON string to deserialize.</param>
    /// <returns>The deserialized dynamic object.</returns>
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