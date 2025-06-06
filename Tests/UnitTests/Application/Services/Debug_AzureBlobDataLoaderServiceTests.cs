using Xunit;
using Moq;
using Azure.Storage.Blobs;
using Azure; // For Response
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Services;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application.Services;

public class Debug_AzureBlobDataLoaderServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Mock<ILogger<AzureBlobDataLoaderService>> _mockLogger;

    public Debug_AzureBlobDataLoaderServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _mockLogger = new Mock<ILogger<AzureBlobDataLoaderService>>();

        _mockLogger.Setup(x => x.Log<object>(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()))
            .Callback((LogLevel logLevel, EventId eventId, object state, Exception exception, Func<object, Exception, string> formatter) =>
            {
                var message = formatter(state, exception);
                _testOutputHelper.WriteLine($"[{logLevel}] {message}");

                if (exception != null)
                {
                    _testOutputHelper.WriteLine($"Exception: {exception.GetType().Name} - {exception.Message}\n{exception.StackTrace}");
                }
            });
    }

    [Fact]
    public async Task Debug_MockBlobClient_DownloadToAsync_WritesToStreamAndCanBeRead()
    {
        // Arrange
        var mockBlobClient = new Mock<BlobClient>();
        var expectedContent = "This is some debug content to test stream writing.";
        var expectedBytes = Encoding.UTF8.GetBytes(expectedContent);

        var simulatedServiceStream = new MemoryStream();

        mockBlobClient.Setup(b => b.DownloadToAsync(
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((destinationStream, ct) =>
            {
                _testOutputHelper.WriteLine($"DEBUG: Callback entered. Destination stream initial position: {destinationStream.Position}");
                _testOutputHelper.WriteLine($"DEBUG: Destination stream initial length: {destinationStream.Length}");

                // Write the bytes to the destination stream
                destinationStream.Write(expectedBytes, 0, expectedBytes.Length);

                _testOutputHelper.WriteLine($"DEBUG: Destination stream position after write: {destinationStream.Position}");
                _testOutputHelper.WriteLine($"DEBUG: Destination stream length after write: {destinationStream.Length}");

                // ✨ FIX APPLIED HERE:
                // Removed the internal reading and assertion to prevent premature disposal of destinationStream.
                // The actual verification happens later in the test using simulatedServiceStream.
                // destinationStream.Position = 0; // No longer needed here
                // using var reader = new StreamReader(destinationStream, Encoding.UTF8);
                // var contentFromCallback = reader.ReadToEnd();
                // _testOutputHelper.WriteLine($"DEBUG: Content read from stream INSIDE CALLBACK: '{contentFromCallback}'");
                // Assert.Equal(expectedContent, contentFromCallback);
            })
            .Returns(Task.FromResult(Mock.Of<Response>()));

        // Act
        await mockBlobClient.Object.DownloadToAsync(simulatedServiceStream, CancellationToken.None);

        // Assert: This is where the stream content is truly verified after the mock call.
        _testOutputHelper.WriteLine($"DEBUG: Outside Callback. Simulated stream position after mock call: {simulatedServiceStream.Position}");
        _testOutputHelper.WriteLine($"DEBUG: Outside Callback. Simulated stream length after mock call: {simulatedServiceStream.Length}");

        simulatedServiceStream.Position = 0; // Rewind before reading
        using var finalReader = new StreamReader(simulatedServiceStream, Encoding.UTF8);
        var finalContent = await finalReader.ReadToEndAsync();

        _testOutputHelper.WriteLine($"DEBUG: Final content read from simulated stream: '{finalContent}'");
        Assert.Equal(expectedContent, finalContent);
    }
}