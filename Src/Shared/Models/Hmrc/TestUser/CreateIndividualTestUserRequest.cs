using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Hmrc.TestUser;

/// <summary>
/// Represents the request body for creating an individual test user with specific service enrolments.
/// </summary>
[SwaggerSchema(Description = "Represents the request body for creating an individual test user with specific service enrolments.")]
public class CreateIndividualTestUserRequest
{
    /// <summary>
    /// A list of HMRC services that the individual test user should be enrolled for.
    /// To create a test user with no services, leave the list empty.
    /// </summary>
    /// <example>
    /// ["mtd-income-tax", "mtd-vat"]
    /// </example>
    [SwaggerSchema(Description = "A list of HMRC services that the individual test user should be enrolled for. To create a test user with no services, leave the list empty.")]
    public List<string> ServiceNames { get; set; } = new List<string>();
}