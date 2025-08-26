using System.Text;
using Application.Services;
using Azure;
using Azure.Storage.Blobs;
using Core.Models.Settings;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using UnitTests.TestHelpers;
using Xunit.Abstractions;

namespace UnitTests.Application.Services
{
    public class AzureBlobDataLoaderServiceTests
    {
        private readonly Mock<IOptions<AzureBlobSettings>> _mockBlobSettingsOptions;
        private readonly Mock<BlobServiceClient> _mockBlobServiceClient;
        private readonly Mock<BlobClient> _mockBlobClient;
        private readonly AzureBlobSettings _testBlobSettings;
        private readonly MockLogger<AzureBlobDataLoaderService> _mockLoggerHelper;

        public AzureBlobDataLoaderServiceTests(ITestOutputHelper testOutputHelper)
        {
            _mockBlobSettingsOptions = new Mock<IOptions<AzureBlobSettings>>();
            _mockBlobServiceClient = new Mock<BlobServiceClient>();
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            _mockBlobClient = new Mock<BlobClient>();

            _testBlobSettings = new AzureBlobSettings
            {
                TaxRatesContainerName = "test-container",
                TaxRatesUkBlobName = "test-blob.json",
                BlobConnectionString = "DefaultEndpointsProtocol=https;AccountName=test;AccountKey=test;EndpointSuffix=core.windows.net" // Example
            };

            _mockBlobSettingsOptions.Setup(o => o.Value).Returns(_testBlobSettings);

            _mockBlobServiceClient
                .Setup(s => s.GetBlobContainerClient(_testBlobSettings.TaxRatesContainerName))
                .Returns(mockBlobContainerClient.Object);

            mockBlobContainerClient
                .Setup(c => c.GetBlobClient(_testBlobSettings.TaxRatesUkBlobName))
                .Returns(_mockBlobClient.Object);

            _mockLoggerHelper = new MockLogger<AzureBlobDataLoaderService>(testOutputHelper);
            
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidParameters_InitializesService()
        {
            // Arrange (already done in constructor)

            // Act
            var service = new AzureBlobDataLoaderService(
                _mockBlobSettingsOptions.Object,
                _mockLoggerHelper.Mock.Object, // ✨ UPDATED: Use the helper's mock object
                _mockBlobServiceClient.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullBlobSettings_ThrowsArgumentNullException()
        {
            // Arrange
            _mockBlobSettingsOptions.Setup(o => o.Value).Returns((AzureBlobSettings)null!); // Explicitly set to null

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new AzureBlobDataLoaderService(
                    _mockBlobSettingsOptions.Object,
                    _mockLoggerHelper.Mock.Object, // ✨ UPDATED
                    _mockBlobServiceClient.Object);
            });
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange (already done in constructor)

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new AzureBlobDataLoaderService(
                    _mockBlobSettingsOptions.Object,
                    null!, // Still pass null directly to test the null logger case
                    _mockBlobServiceClient.Object);
            });
        }

        [Fact]
        public void Constructor_WithNullBlobServiceClient_ThrowsArgumentNullException()
        {
            // Arrange (already done in constructor)

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new AzureBlobDataLoaderService(
                    _mockBlobSettingsOptions.Object,
                    _mockLoggerHelper.Mock.Object, // ✨ UPDATED
                    null!); // Pass null BlobServiceClient
            });
        }

        [Fact]
        public void Constructor_WithEmptyContainerName_ThrowsArgumentException()
        {
            // Arrange
            _testBlobSettings.TaxRatesContainerName = ""; // Set container name to empty

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                new AzureBlobDataLoaderService(
                    _mockBlobSettingsOptions.Object,
                    _mockLoggerHelper.Mock.Object, // ✨ UPDATED
                    _mockBlobServiceClient.Object);
            });
            Assert.Contains("Container name cannot be null or empty", exception.Message);
        }

        [Fact]
        public void Constructor_WithEmptyBlobName_ThrowsArgumentException()
        {
            // Arrange
            _testBlobSettings.TaxRatesUkBlobName = ""; // Set blob name to empty

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                new AzureBlobDataLoaderService(
                    _mockBlobSettingsOptions.Object,
                    _mockLoggerHelper.Mock.Object, // ✨ UPDATED
                    _mockBlobServiceClient.Object);
            });
            Assert.Contains("Blob name cannot be null or empty", exception.Message);
        }

        #endregion

        #region LoadBlobDataAsync Tests

        [Fact]
        public async Task LoadBlobDataAsync_ReturnsBlobContent_Successfully()
        {
            // Arrange
            var expectedContent = "This is the content of the blob.";
            var expectedBytes = Encoding.UTF8.GetBytes(expectedContent);

            _mockBlobClient.Setup(b => b.DownloadToAsync(
                    It.IsAny<Stream>()))
                .Callback<Stream>((destinationStream) =>
                {
                    destinationStream.Write(expectedBytes, 0, expectedBytes.Length);
                })
                .Returns(Task.FromResult(Mock.Of<Response>()));

            var service = new AzureBlobDataLoaderService(
                _mockBlobSettingsOptions.Object,
                _mockLoggerHelper.Mock.Object, // ✨ UPDATED: Use the helper's mock object
                _mockBlobServiceClient.Object);

            // Act
            var result = await service.LoadBlobDataAsync();

            // Assert
            Assert.Equal(expectedContent, result);

            // ✨ UPDATED: Use the helper's verification methods
            _mockLoggerHelper.VerifyInformation("Data successfully fetched from Azure Blob Storage.");
        }

        [Fact]
        public async Task LoadBlobDataAsync_HandlesEmptyBlobContent()
        {
            // Arrange
            var expectedContent = ""; // Empty string
            var expectedBytes = Encoding.UTF8.GetBytes(expectedContent);

            _mockBlobClient.Setup(b => b.DownloadToAsync(It.IsAny<Stream>()))
                .Callback<Stream>((destinationStream) =>
                {
                    destinationStream.Write(expectedBytes, 0, expectedBytes.Length);
                })
                .Returns(Task.FromResult(Mock.Of<Response>()));

            var service = new AzureBlobDataLoaderService(
                _mockBlobSettingsOptions.Object,
                _mockLoggerHelper.Mock.Object, // ✨ UPDATED
                _mockBlobServiceClient.Object);

            // Act
            var result = await service.LoadBlobDataAsync();

            // Assert
            Assert.Equal(expectedContent, result);

            // ✨ UPDATED
            _mockLoggerHelper.VerifyInformation("Data successfully fetched from Azure Blob Storage.");
        }

        [Fact]
        public async Task LoadBlobDataAsync_ThrowsException_WhenDownloadFails()
        {
            // Arrange
            _mockBlobClient.Setup(b => b.DownloadToAsync(
                It.IsAny<Stream>()))
                .ThrowsAsync(new InvalidOperationException("Simulated download failure."));

            var service = new AzureBlobDataLoaderService(
                _mockBlobSettingsOptions.Object,
                _mockLoggerHelper.Mock.Object, // ✨ UPDATED
                _mockBlobServiceClient.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.LoadBlobDataAsync());
            Assert.Contains("Simulated download failure.", exception.Message);

            // ✨ UPDATED
            _mockLoggerHelper.VerifyError("Failed to fetch data from Azure Blob Storage.");
        }

        #endregion
    }
}