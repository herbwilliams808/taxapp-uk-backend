using System.Text.Json;
using System.Text.Json.Serialization;
using API.SwaggerExamples;
using Application.Calculators;
using Application.Services;
using Application.Services.HmrcIntegration.Auth;
using Application.Services.HmrcIntegration.TestUser;
using Application.Services.HmrcIntegration.Tutorial;
using Core.Interfaces.Calculators;
using Core.Interfaces.HmrcIntegration.Auth;
using Core.Interfaces.HmrcIntegration.TestUser;
using Core.Interfaces.HmrcIntegration.Tutorial;
using Core.Interfaces.Http;
using Core.Interfaces.Services;
using Core.Models.Settings;
using Core.Services;
using Infrastructure.Http;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

// Make sure this is present for your controllers

namespace API;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddAzureWebAppDiagnostics();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);

        // Load configuration
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Register services
        builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));

        // Constants for HttpClient names
        const string HmrcAuthClientName = "HMRCAuthClient";
        const string HmrcApiClientName = "HMRCApiClient";


        // Register HttpClient for HMRC Auth Service (for token endpoint)
        builder.Services.AddHttpClient(HmrcAuthClientName, client =>
        {
            var tokenUrl = builder.Configuration.GetValue<string>("HMRC_TestApi:TokenUrl") ?? "https://test-api.service.hmrc.gov.uk/oauth/token";
            client.BaseAddress = new Uri(tokenUrl);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });

        // Register HMRC Auth Service (for application-restricted tokens)
        builder.Services.AddScoped<IHmrcAuthService, HmrcAuthService>(provider =>
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(HmrcAuthClientName);
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new HmrcAuthService(httpClient, configuration);
        });

        // Register HMRC User Auth Service (for user-restricted tokens)
        builder.Services.AddScoped<IHmrcUserAuthService, HmrcUserAuthService>(provider =>
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(HmrcAuthClientName);
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new HmrcUserAuthService(httpClient, configuration);
        });

        // Register HttpClient for HMRC Test API Service (for test user creation and Hello World/Application/User)
        builder.Services.AddHttpClient(HmrcApiClientName, client =>
        {
            var baseUrl = builder.Configuration.GetValue<string>("HMRC_TestApi:BaseUrl");
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("HMRC_TestApi:BaseUrl is not configured in appsettings.json.");
            }

            client.BaseAddress = new Uri(baseUrl);
            // The default Accept header is now set in the HmrcClient constructor for better encapsulation.
        });

        // Register the new HmrcClient which abstracts HttpClient usage for API calls
        builder.Services.AddScoped<IApiClient, ApiClient>(provider =>
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(HmrcApiClientName);
            return new ApiClient(httpClient);
        });

        // Register HMRC Test User Service
        builder.Services.AddScoped<IHmrcTestUserService, HmrcTestUserService>(provider =>
        {
            var hmrcClient = provider.GetRequiredService<IApiClient>();
            var hmrcAuthService = provider.GetRequiredService<IHmrcAuthService>();
            return new HmrcTestUserService(hmrcClient, hmrcAuthService);
        });

        // Register HmrcTutorialService (now includes IHmrcUserAuthService dependency)
        builder.Services.AddScoped<IHmrcTutorialService, HmrcTutorialService>(provider =>
        {
            var hmrcClient = provider.GetRequiredService<IApiClient>();
            var hmrcAuthService = provider.GetRequiredService<IHmrcAuthService>();
            var hmrcUserAuthService = provider.GetRequiredService<IHmrcUserAuthService>(); // Get the user auth service
            return new HmrcTutorialService(hmrcClient, hmrcAuthService, hmrcUserAuthService); // Pass all dependencies
        });


        builder.Services.AddSingleton<IBlobDataLoaderService, AzureBlobDataLoaderService>();
        builder.Services.AddSingleton<IJsonDeserialiserService, JsonDeserialiserService>();
        builder.Services.AddSingleton<ITaxRatesCacheService, TaxRatesCacheService>();

        builder.Services.AddScoped<ITotalEmploymentIncomeCalculator, TotalEmploymentIncomeCalculator>();
        builder.Services.AddScoped<ITotalEmploymentBenefitsCalculator, TotalEmploymentBenefitsCalculator>();
        builder.Services.AddScoped<ITotalEmploymentExpensesCalculator, TotalEmploymentExpensesCalculator>();
        builder.Services.AddScoped<ITotalOtherDeductionsCalculator, TotalOtherDeductionsCalculator>();
        builder.Services.AddScoped<IProfitFromPropertiesCalculator, ProfitFromPropertiesCalculator>();
        builder.Services.AddScoped<ITotalIncomeCalculator, TotalIncomeCalculator>();
        builder.Services.AddScoped<IGiftAidPaymentsCalculator, GiftAidPaymentsCalculator>();
        builder.Services.AddScoped<IBasicRateLimitCalculator, BasicRateLimitCalculator>();
        builder.Services.AddScoped<IAdjustedNetIncomeCalculator, AdjustedNetIncomeCalculator>();
        builder.Services.AddScoped<IPersonalAllowanceCalculator, PersonalAllowanceCalculator>();
        builder.Services.AddScoped<ITaxOwedCalculator, TaxOwedCalculator>();
        builder.Services.AddScoped<TaxEstimationService>();


        // Add CORS configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:8081",  // Expo web development server
                        "https://localhost:8081", // HTTPS variant
                        "exp://",                 // Expo Go app
                        "http://localhost:19000", // Alternative Expo port
                        "http://localhost:19001", // Alternative Expo port
                        "http://localhost:19002", // Alternative Expo port
                        "https://localhost:19000",
                        "https://localhost:19001", 
                        "https://localhost:19002"
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowedToAllowWildcardSubdomains(); // Allow Expo Go dynamic origins
            });
        });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
            c.ExampleFilters();
        });

        builder.Services.AddSwaggerExamplesFromAssemblyOf<TaxEstimationRequestExample>();

        var app = builder.Build();

        // --- STARTUP LOGGING AND TAX RATES LOADING ---
        var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();

        startupLogger.LogInformation(
            "### PROGRAM.CS: APPLICATION STARTUP LOG: Trying to log to Azure filesystem. ###");
        startupLogger.LogInformation("PROGRAM.CS: Application startup begins...");

        try
        {
            var blobLoader = app.Services.GetRequiredService<IBlobDataLoaderService>();
            var jsonDeserializer = app.Services.GetRequiredService<IJsonDeserialiserService>();
            var cacheService = app.Services.GetRequiredService<ITaxRatesCacheService>();

            startupLogger.LogInformation("PROGRAM.CS: Attempting to load tax rates from blob at startup.");
            var jsonString = await blobLoader.LoadBlobDataAsync();
            startupLogger.LogInformation(
                $"PROGRAM.CS: Successfully loaded {jsonString.Length} bytes from blob storage.");

            using var jsonDoc = jsonDeserializer.Deserialise<JsonDocument>(jsonString);
            startupLogger.LogInformation("PROGRAM.CS: Successfully deserialized JSON document.");

            cacheService.LoadCache(jsonDoc.RootElement);

            startupLogger.LogInformation("PROGRAM.CS: Tax rates cache loaded successfully at startup.");
        }
        catch (Exception ex)
        {
            startupLogger.LogError(ex,
                "PROGRAM.CS: FATAL ERROR during application startup: Failed to load tax rates cache. Application will terminate.");
            throw;
        }
        // --- END STARTUP LOGGING AND TAX RATES LOADING ---


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Use CORS
        app.UseCors("AllowFrontend");

        app.MapControllers();

        app.Run();
    }
}