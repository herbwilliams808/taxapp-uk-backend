using System.Text.Json;
using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

public class JsonDeserializerServiceTests
{
    private readonly JsonDeserializerService _service;
    private readonly Mock<ILogger<JsonDeserializerService>> _loggerMock;

    public JsonDeserializerServiceTests()
    {
        _loggerMock = new Mock<ILogger<JsonDeserializerService>>();
        _service = new JsonDeserializerService(_loggerMock.Object);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsExpectedObject()
    {
        // Arrange
        var json = "{\"Key\":\"Value\"}";

        // Act
        var result = _service.Deserialize<Dictionary<string, string>>(json);

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
        var ex = Assert.Throws<JsonException>(() => _service.Deserialize<Dictionary<string, string>>(invalidJson));
        Assert.NotNull(ex);
    }

    [Fact]
    public void Deserialize_EmptyJson_ThrowsArgumentException()
    {
        // Arrange
        var emptyJson = "";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.Deserialize<Dictionary<string, string>>(emptyJson));
        Assert.Equal("JSON content cannot be null or empty. (Parameter 'jsonContent')", ex.Message);
    }

    [Fact]
    public void DeserializeDynamic_ValidJson_ReturnsJsonElement()
    {
        // Arrange
        var json = "{\"Key\":\"Value\"}";

        // Act
        var result = _service.DeserializeDynamic(json);

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
        var ex = Assert.Throws<ArgumentException>(() => _service.DeserializeDynamic(emptyJson));
        Assert.Equal("JSON content cannot be null or empty. (Parameter 'jsonContent')", ex.Message);
    }
}
