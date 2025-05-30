using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class JsonDeserializerService
    {
        private readonly ILogger<JsonDeserializerService> _logger;

        public JsonDeserializerService(ILogger<JsonDeserializerService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Deserializes JSON content into a strongly-typed object.
        /// </summary>
        /// <typeparam name="T">The type to which the JSON should be deserialized.</typeparam>
        /// <param name="jsonContent">The JSON string to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        public T Deserialize<T>(string jsonContent)
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
        public object? DeserializeDynamic(string jsonContent)
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
}
