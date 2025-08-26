using System.Text.Json;
using Application.Services;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Application.Services;

public class JsonDeserialiserServiceTests
{
    private readonly JsonDeserialiserService _service;
    private readonly Mock<ILogger<JsonDeserialiserService>> _loggerMock;

    public JsonDeserialiserServiceTests()
    {
        _loggerMock = new Mock<ILogger<JsonDeserialiserService>>();
        _service = new JsonDeserialiserService(_loggerMock.Object);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsExpectedObject()
    {
        // Arrange
        var json = "{\"Key\":\"Value\"}";

        // Act
        var result = _service.Deserialise<Dictionary<string, string>>(json);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ContainsKey("Key"));
        Assert.Equal("Value", result["Key"]);
    }

    [Fact]
    public void Deserialize_InvalidJson_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = "{invalid json}";

        // Act & Assert
        var ex = Assert.Throws<JsonException>(() => _service.Deserialise<Dictionary<string, string>>(invalidJson));
        Assert.NotNull(ex);
    }

    [Fact]
    public void Deserialize_EmptyJson_ThrowsArgumentException()
    {
        // Arrange
        var emptyJson = "";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.Deserialise<Dictionary<string, string>>(emptyJson));
        Assert.Equal("JSON content cannot be null or empty. (Parameter 'jsonContent')", ex.Message);
    }

    [Fact]
    public void DeserializeDynamic_ValidJson_ReturnsJsonElement()
    {
        // Arrange
        var json = "{\"Key\":\"Value\"}";

        // Act
        var result = _service.DeserialiseDynamic(json);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<JsonElement>(result);
        
        var jsonElement = (JsonElement)result!;
        Assert.True(jsonElement.TryGetProperty("Key", out var value));
        Assert.Equal("Value", value.GetString());
    }

    [Fact]
    public void DeserializeDynamic_EmptyJson_ThrowsArgumentException()
    {
        // Arrange
        var emptyJson = "";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.DeserialiseDynamic(emptyJson));
        Assert.Equal("JSON content cannot be null or empty. (Parameter 'jsonContent')", ex.Message);
    }
}