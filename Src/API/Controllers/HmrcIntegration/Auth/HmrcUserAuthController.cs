using System.Collections.Concurrent;
using Application.Interfaces.Services.HmrcIntegration.Auth;
using Application.Interfaces.Services.HmrcIntegration.TestUser;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
// For Query.SingleOrDefault

// For ConcurrentDictionary

namespace API.Controllers.HmrcIntegration.Auth;

[ApiController]
[Route("api/hmrc/user-auth")] // Base route for user authentication operations
[SwaggerTag("Operations for HMRC User-Restricted (OAuth 2.0 Authorization Code Grant) authentication.")]
public class HmrcUserAuthController : ControllerBase
{
    private readonly IHmrcUserAuthService _hmrcUserAuthService;
    private readonly ILogger<HmrcUserAuthController> _logger;

    // In a real application, you'd want a more robust way to manage 'state' and session IDs.
    // For this tutorial, we'll use a simple in-memory approach.
    private static readonly ConcurrentDictionary<string, string> _pendingStates = new ConcurrentDictionary<string, string>();


    public HmrcUserAuthController(
        IHmrcUserAuthService hmrcUserAuthService,
        ILogger<HmrcUserAuthController> logger)
    {
        _hmrcUserAuthService = hmrcUserAuthService;
        _logger = logger;
    }

    /// <summary>
    /// Initiates the HMRC user authorization flow.
    /// </summary>
    /// <remarks>
    /// Redirects the user to the HMRC Government Gateway login page to authorize your application.
    /// After successful authorization, HMRC will redirect back to the configured RedirectUri.
    /// </remarks>
    /// <param name="scopes">The OAuth 2.0 scopes to request (e.g., "hello").</param>
    /// <param name="userId">A unique identifier for the user session (e.g., a GUID or session ID). This will be used to store tokens.</param>
    /// <returns>A redirect to the HMRC authorization page.</returns>
    [HttpGet("authorize")]
    [SwaggerOperation(
        Summary = "Initiate HMRC user authorization.",
        Description = "Redirects the user to HMRC for login and consent for user-restricted API access. Specify 'hello' for the tutorial endpoint."
    )]
    [SwaggerResponse(302, "Redirects to HMRC authorization URL.")]
    [SwaggerResponse(500, "Error generating authorization URL.")]
    public IActionResult AuthorizeUser([FromQuery] string scopes = "hello", [FromQuery] string? userId = null)
    {
        try
        {
            // In a real application, userId would come from your session management.
            // For tutorial, if not provided, generate a simple one.
            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString(); // Simple session ID for demo
                _logger.LogInformation("No userId provided, generating a new one: {UserId}", userId);
            }

            // 'state' is crucial for CSRF protection and maintaining state.
            // For a demo, a simple GUID is used. In production, link it to user's session.
            var state = Guid.NewGuid().ToString();
            _pendingStates[state] = userId; // Store the state for later verification

            var authUrl = _hmrcUserAuthService.GetAuthorizationUrl(scopes, state);
            _logger.LogInformation("Redirecting user {UserId} to HMRC authorization URL: {AuthUrl}", userId, authUrl);
            return Redirect(authUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating HMRC authorization URL: {Message}", ex.Message);
            return StatusCode(500, new { error = "Failed to generate authorization URL.", detail = ex.Message });
        }
    }

    /// <summary>
    /// HMRC redirects back to this endpoint after user authorization.
    /// </summary>
    /// <remarks>
    /// This endpoint receives the authorization code from HMRC and exchanges it for access and refresh tokens.
    /// It then stores these tokens temporarily for the associated user session.
    /// </remarks>
    /// <param name="code">The authorization code provided by HMRC.</param>
    /// <param name="state">The state parameter originally sent in the authorization request.</param>
    /// <returns>A confirmation message or redirect.</returns>
    [HttpGet("callback")]
    [SwaggerOperation(
        Summary = "HMRC Authorization Callback.",
        Description = "Receives the authorization code from HMRC and exchanges it for tokens. This endpoint is called by HMRC after user consent."
    )]
    [SwaggerResponse(200, "Successfully obtained and stored user tokens.", typeof(string))]
    [SwaggerResponse(400, "Invalid state or missing authorization code.")]
    [SwaggerResponse(500, "Error exchanging authorization code for tokens.")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
    {
        _logger.LogInformation("HMRC callback received. Code: {Code}, State: {State}", code, state);

        if (!_pendingStates.TryRemove(state, out var userId) || string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Invalid or missing state in HMRC callback. State: {State}", state);
            return BadRequest(new { error = "Invalid or expired state parameter. Possible CSRF attack or session mismatch." });
        }

        if (string.IsNullOrEmpty(code))
        {
            // HMRC can also return 'error' and 'error_description' in query string if user denies consent
            var error = HttpContext.Request.Query["error"].SingleOrDefault();
            var errorDescription = HttpContext.Request.Query["error_description"].SingleOrDefault();
            if (!string.IsNullOrEmpty(error))
            {
                 _logger.LogError("HMRC callback error: {Error} - {ErrorDescription}", error, errorDescription);
                 return BadRequest(new { error = $"HMRC authorization denied or failed: {error}", detail = errorDescription });
            }

            _logger.LogWarning("HMRC callback received without authorization code. State: {State}", state);
            return BadRequest(new { error = "Authorization code missing from callback." });
        }

        try
        {
            var tokenResponse = await _hmrcUserAuthService.ExchangeCodeForTokensAsync(code);
            _hmrcUserAuthService.StoreUserTokens(tokenResponse, userId);
            _logger.LogInformation("Successfully exchanged code for tokens and stored for user {UserId}. Access Token: {TokenPrefix}...", userId, tokenResponse.AccessToken?.Substring(0, Math.Min(tokenResponse.AccessToken.Length, 10)));

            // You might want to redirect the user to a "success" page or return a user-friendly message
            return Ok($"Successfully authenticated user {userId}. Access token received and stored. You can now call user-restricted endpoints using this userId.");
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HttpRequestException during token exchange for user {UserId}: {Message}", userId, httpEx.Message);
            return StatusCode((int?)(httpEx.StatusCode) ?? 500, new { error = "Failed to exchange authorization code for tokens.", detail = httpEx.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during token exchange for user {UserId}: {Message}", userId, ex.Message);
            return StatusCode(500, new { error = "An internal server error occurred during token exchange.", detail = ex.Message });
        }
    }
}