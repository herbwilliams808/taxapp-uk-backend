using Application.Services;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Models.Settings;

namespace UnitTests.Application.Services;

public class AzureBlobDataLoaderServiceTests
{
    private readonly Mock<IOptions<AzureBlobSettings>> _mockBlobSettings;
    private readonly Mock<ILogger<AzureBlobDataLoaderService>> _mockLogger;
    private readonly Mock<BlobServiceClient> _mockBlobServiceClient;

    public AzureBlobDataLoaderServiceTests()
    {
        _mockBlobSettings = new Mock<IOptions<AzureBlobSettings>>();
        _mockLogger = new Mock<ILogger<AzureBlobDataLoaderService>>();
        _mockBlobServiceClient = new Mock<BlobServiceClient>();
        var mockContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();

        // Default setup for mock settings to be used across tests
        _mockBlobSettings.Setup(o => o.Value).Returns(new AzureBlobSettings
        {
            BlobConnectionString = "DefaultConnectionString", // This won't be used by the mocked client
            TaxRatesContainerName = "tax-rates-container",
            TaxRatesUkBlobName = "tax-rates.json"
        });

        // Default setup for the BlobServiceClient chain
        _mockBlobServiceClient
            .Setup(b => b.GetBlobContainerClient(It.IsAny<string>()))
            .Returns(mockContainerClient.Object);

        mockContainerClient
            .Setup(c => c.GetBlobClient(It.IsAny<string>()))
            .Returns(mockBlobClient.Object);
    }

    // The tests LoadBlobDataAsync_ShouldReturnBlobContent_OnSuccess and LoadBlobDataAsync_ShouldThrowException_OnBlobNotFound
    // have been omitted as requested, assuming their functionality works in practice.

    [Fact]
    public void Constructor_ShouldUseProvidedBlobServiceClient()
    {
        // Arrange
        var mockSettings = new Mock<IOptions<AzureBlobSettings>>();
        // Ensure all required members of AzureBlobSettings are set
        mockSettings.Setup(o => o.Value).Returns(new AzureBlobSettings {
            BlobConnectionString = "", // The required member can be empty for this test as it's mocked
            TaxRatesContainerName = "test-container",
            TaxRatesUkBlobName = "test-blob"
        });
        var mockLogger = new Mock<ILogger<AzureBlobDataLoaderService>>();
        var mockBlobServiceClient = new Mock<BlobServiceClient>(); // A fresh mock for this test

        // Act
        var service = new AzureBlobDataLoaderService(mockSettings.Object, mockLogger.Object, mockBlobServiceClient.Object);

        // Assert
        Assert.NotNull(service); // Ensure no exceptions during construction
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenBlobSettingsIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AzureBlobDataLoaderService(null!, _mockLogger.Object, _mockBlobServiceClient.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AzureBlobDataLoaderService(_mockBlobSettings.Object, null!, _mockBlobServiceClient.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenBlobServiceClientIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AzureBlobDataLoaderService(_mockBlobSettings.Object, _mockLogger.Object, null!));
    }

    [Fact]
    public void Constructor_Secondary_ShouldThrowArgumentNullException_WhenBlobSettingsIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AzureBlobDataLoaderService(null!, _mockLogger.Object));
    }
}