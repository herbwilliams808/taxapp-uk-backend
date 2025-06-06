// In your test project (e.g., TaxApp.Tests/CustomWebApplicationFactory.cs)

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Assuming your API project's startup is in the 'API' namespace

namespace IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Here you can override services for testing purposes.
            // For example, if your TaxEstimationService has a dependency on a database,
            // you might replace it with an in-memory database or a mock.

            // Example: Remove any production IHostedService if it's not needed for tests
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(Microsoft.Extensions.Hosting.IHostedService));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // If you have a real database, you'd typically replace its registration
            // with an in-memory equivalent or a dedicated test database connection.
            // For this example, assuming TaxEstimationService is computational or uses mocks configured elsewhere.

            // Configure logging to be minimal during tests, or to a test-specific sink
            services.AddLogging(builder =>
            {
                builder.ClearProviders(); // Clear default providers
                builder.AddConsole(); // Add a console logger for test output if needed
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Warning); // Adjust log level for tests
            });
        });

        // You can also configure the environment (e.g., "Development", "Testing")
        builder.UseEnvironment("Testing");
    }
}