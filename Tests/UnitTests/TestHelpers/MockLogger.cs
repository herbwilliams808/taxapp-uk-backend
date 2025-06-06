using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

// ✨ NEW: Add this for LINQ's .Any() and .Contains()

namespace UnitTests.TestHelpers;

public class MockLogger<TService> where TService : class
{
    public Mock<ILogger<TService>> Mock { get; }
    public List<string> LoggedMessages { get; }

    private readonly ITestOutputHelper _testOutputHelper;

    public MockLogger(ITestOutputHelper testOutputHelper = null)
    {
        Mock = new Mock<ILogger<TService>>();
        LoggedMessages = new List<string>();
        _testOutputHelper = testOutputHelper;

        SetupLoggerMock();
    }

    private void SetupLoggerMock()
    {
        Mock.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
            .Callback(new InvocationAction(invocation =>
            {
                var logLevel = (LogLevel)invocation.Arguments[0];
                var eventId = (EventId)invocation.Arguments[1];
                var state = invocation.Arguments[2];
                var exception = invocation.Arguments[3] as Exception;
                var formatter = invocation.Arguments[4] as Delegate;

                string message = string.Empty;
                if (formatter != null)
                {
                    message = (string)formatter.DynamicInvoke(state, exception);
                }
                else
                {
                    message = state?.ToString() ?? string.Empty;
                }

                _testOutputHelper?.WriteLine($"[{logLevel}] {message}");
                LoggedMessages.Add(message);
            }));
    }

    public void VerifyLog(string messageContent, LogLevel level, Times? times = null)
    {
        // ✨ FIX: Use LINQ's Any() with string.Contains() for flexible substring matching
        if (times.HasValue && times.Value == Times.Once())
        {
            // For Times.Once(), we need exactly one matching message
            Xunit.Assert.Single(LoggedMessages, logMsg => logMsg.Contains(messageContent));
        }
        else if (times.HasValue && times.Value == Times.Never())
        {
            // For Times.Never(), ensure no message contains the content
            Xunit.Assert.DoesNotContain(LoggedMessages, logMsg => logMsg.Contains(messageContent));
        }
        else // Default to AtLeastOnce behavior if no specific Times is provided or for generic assertion
        {
            // For AtLeastOnce, ensure at least one message contains the content
            Xunit.Assert.True(
                LoggedMessages.Any(logMsg => logMsg.Contains(messageContent)),
                $"Log message containing '{messageContent}' not found in captured logs."
            );
        }

        // Then, use Moq's Verify to check the raw log invocation count and level.
        Mock.Verify(x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            times ?? Times.AtLeastOnce());
    }

    public void VerifyInformation(string messageContent, Times? times = null)
    {
        VerifyLog(messageContent, LogLevel.Information, times);
    }

    public void VerifyError(string messageContent, Times? times = null)
    {
        VerifyLog(messageContent, LogLevel.Error, times);
    }
}