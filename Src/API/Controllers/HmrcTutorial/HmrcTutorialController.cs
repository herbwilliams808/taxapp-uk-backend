using System.Threading.Tasks;
using Application.Interfaces.Services.HmrcTutorial;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Logging;
using System;

namespace API.Controllers.HMRCTutorial;

[ApiController]
[Route("api/hmrc")] // General route for HMRC tutorial endpoints
[SwaggerTag("Operations related to basic HMRC API connectivity tests.")]
public class HmrcTutorialController : ControllerBase
{
    private readonly IHmrcTutorialService _hmrcTutorialService;
    private readonly ILogger<HmrcTutorialController> _logger;

    public HmrcTutorialController(
        IHmrcTutorialService hmrcTutorialService,
        ILogger<HmrcTutorialController> logger)
    {
        _hmrcTutorialService = hmrcTutorialService;
        _logger = logger;
    }

    /// <summary>
    /// Calls the HMRC "Hello World" test endpoint. (No Auth)
    /// </summary>
    /// <remarks>
    /// This endpoint demonstrates basic connectivity to the HMRC test API by calling the simple /hello/world endpoint, which does not require authentication.
    /// </remarks>
    /// <returns>The raw string response from the HMRC Hello World API.</returns>
    [HttpGet("hello-world")]
    [SwaggerOperation(
        Summary = "Call HMRC Hello World endpoint (No Auth).",
        Description = "Tests basic connectivity to the HMRC test API without authentication."
    )]
    [SwaggerResponse(200, "Successfully received response from Hello World API.", typeof(string))]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<ActionResult<string>> GetHelloWorld()
    {
        try
        {
            _logger.LogInformation("Attempting to call HMRC Hello World endpoint.");
            var response = await _hmrcTutorialService.GetHelloWorldAsync();
            _logger.LogInformation("Successfully received response from Hello World: {Response}", response);
            return Ok(response);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HttpRequestException while calling Hello World API: {Message}", httpEx.Message);
            return StatusCode((int?)(httpEx.StatusCode) ?? 500, new { error = httpEx.Message, detail = httpEx.InnerException?.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while calling Hello World API: {Message}", ex.Message);
            return StatusCode(500, new { error = "An internal server error occurred.", detail = ex.Message });
        }
    }

    /// <summary>
    /// Calls the HMRC "Hello Application" test endpoint. (Application Auth)
    /// </summary>
    /// <remarks>
    /// This endpoint demonstrates basic connectivity and **application-level OAuth 2.0 authentication** with the HMRC test API by calling the /hello/application endpoint.
    /// </remarks>
    /// <returns>The raw string response from the HMRC Hello Application API.</returns>
    [HttpGet("hello-application")]
    [SwaggerOperation(
        Summary = "Call HMRC Hello Application endpoint (Application Auth).",
        Description = "Tests basic connectivity and application-level OAuth 2.0 authentication to the HMRC test API."
    )]
    [SwaggerResponse(200, "Successfully received response from Hello Application API.", typeof(string))]
    [SwaggerResponse(401, "Authentication failed (invalid or missing token).")]
    [SwaggerResponse(403, "Forbidden access (e.g., incorrect permissions).")]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<ActionResult<string>> GetHelloApplication()
    {
        try
        {
            _logger.LogInformation("Attempting to call HMRC Hello Application endpoint.");
            var response = await _hmrcTutorialService.GetHelloApplicationAsync();
            _logger.LogInformation("Successfully received response from Hello Application: {Response}", response);
            return Ok(response);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HttpRequestException while calling Hello Application API: {Message}", httpEx.Message);
            return StatusCode((int?)(httpEx.StatusCode) ?? 500, new { error = httpEx.Message, detail = httpEx.InnerException?.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while calling Hello Application API: {Message}", ex.Message);
            return StatusCode(500, new { error = "An internal server error occurred.", detail = ex.Message });
        }
    }

    /// <summary>
    /// Calls the HMRC "Hello User" test endpoint. (User Auth)
    /// </summary>
    /// <remarks>
    /// This endpoint demonstrates calling a user-restricted HMRC test API endpoint.
    /// It requires a user-specific OAuth 2.0 access token, which must first be obtained
    /// via the /api/hmrc/user-auth/authorize and /api/hmrc/user-auth/callback flow.
    /// </remarks>
    /// <param name="userId">The unique identifier for the user whose access token should be used (obtained from the /user-auth/callback).</param>
    /// <returns>The raw string response from the HMRC Hello User API.</returns>
    [HttpGet("hello-user")] // New endpoint
    [SwaggerOperation(
        Summary = "Call HMRC Hello User endpoint (User Auth).",
        Description = "Tests calling a user-restricted HMRC API endpoint using a user's OAuth 2.0 access token. Requires a userId obtained from the user authorization flow."
    )]
    [SwaggerResponse(200, "Successfully received response from Hello User API.", typeof(string))]
    [SwaggerResponse(400, "User not authorized or userId missing.")]
    [SwaggerResponse(401, "Authentication failed (invalid or expired user token).")]
    [SwaggerResponse(403, "Forbidden access (e.g., user lacks correct permissions).")]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<ActionResult<string>> GetHelloUser([FromQuery] string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest(new { error = "userId is required to call a user-restricted endpoint." });
        }

        try
        {
            _logger.LogInformation("Attempting to call HMRC Hello User endpoint for userId: {UserId}", userId);
            var response = await _hmrcTutorialService.GetHelloUserAsync(userId); // Call the service method
            _logger.LogInformation("Successfully received response from Hello User for userId {UserId}: {Response}", userId, response);
            return Ok(response);
        }
        catch (InvalidOperationException ioEx) // Catch specific error if user not authorized
        {
            _logger.LogWarning("InvalidOperationException calling Hello User for userId {UserId}: {Message}", userId, ioEx.Message);
            return BadRequest(new { error = ioEx.Message });
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HttpRequestException while calling Hello User API for userId {UserId}: {Message}", userId, httpEx.Message);
            return StatusCode((int?)(httpEx.StatusCode) ?? 500, new { error = httpEx.Message, detail = httpEx.InnerException?.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while calling Hello User API for userId {UserId}: {Message}", userId, ex.Message);
            return StatusCode(500, new { error = "An internal server error occurred.", detail = ex.Message });
        }
    }
}