using Core.Interfaces.HmrcIntegration.TestUser;
using Core.Models.HmrcIntegration.TestUser;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// Required for ILogger

// Required for Exception

namespace API.Controllers.HmrcIntegration.TestUser;

[ApiController]
[Route("api/hmrc/test-users")] // Specific route for test user operations
[SwaggerTag("Operations related to HMRC Test Users for MTD API development.")]
public class HmrcTestUserController(
    IHmrcTestUserService hmrcTestUserService,
    ILogger<HmrcTestUserController> logger)
    : ControllerBase
{
    /// <summary>
    /// Creates a new individual test user for HMRC MTD API access.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to generate a test individual with specific service enrolments
    /// for use with HMRC's Making Tax Digital (MTD) developer APIs.
    /// The response will include the MTD ID, Government Gateway ID, and temporary password.
    /// </remarks>
    /// <param name="request">Details of the individual test user to create, including desired service enrolments.</param>
    /// <returns>A <see cref="CreateIndividualTestUserResponse"/> object with the details of the newly created test user.</returns>
    [HttpPost("individual")]
    [SwaggerOperation(
        Summary = "Create a new individual test user.",
        Description = "Creates a test user representing an individual taxpayer for MTD API development purposes. Optionally specify service enrolments."
    )]
    [SwaggerResponse(200, "Successfully created an individual test user.", typeof(CreateIndividualTestUserResponse))]
    [SwaggerResponse(400, "Invalid request payload or service names provided.")]
    [SwaggerResponse(500, "An unexpected error occurred while creating the test user.")]
    public async Task<ActionResult<CreateIndividualTestUserResponse>> CreateIndividualTestUser(
        [SwaggerRequestBody("The details for creating the individual test user.", Required = true)] CreateIndividualTestUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            logger.LogInformation("Attempting to create individual test user with services: {ServiceNames}", string.Join(", ", request.ServiceNames));
            var response = await hmrcTestUserService.CreateIndividualTestUserAsync(request);
            logger.LogInformation("Successfully created individual test user with MTD ID: {MtdId}", response.MtdId);
            return Ok(response);
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HttpRequestException while creating individual test user: {Message}", httpEx.Message);
            // If StatusCode is null, default to 500
            return StatusCode((int?)(httpEx.StatusCode) ?? 500, new { error = httpEx.Message, detail = httpEx.InnerException?.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while creating individual test user: {Message}", ex.Message);
            return StatusCode(500, new { error = "An internal server error occurred.", detail = ex.Message });
        }
    }
}