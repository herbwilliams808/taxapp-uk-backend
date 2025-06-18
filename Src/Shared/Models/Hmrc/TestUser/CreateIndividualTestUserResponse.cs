using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Hmrc.TestUser;

/// <summary>
/// Represents the response received after successfully creating an individual test user.
/// </summary>
[SwaggerSchema(Description = "Represents the response received after successfully creating an individual test user.")]
public class CreateIndividualTestUserResponse
{
    /// <summary>
    /// The MTD ID (Making Tax Digital ID) for the newly created individual test user.
    /// This ID is crucial for interacting with MTD services for this user.
    /// </summary>
    /// <example>XAMTD00000000001</example>
    [SwaggerSchema(Description = "The MTD ID for the newly created individual test user.")]
    public string MtdId { get; set; } = string.Empty;

    /// <summary>
    /// The Government Gateway ID for the newly created individual test user.
    /// This ID is used for login purposes in the Government Gateway.
    /// </summary>
    /// <example>gateway12345678</example>
    [SwaggerSchema(Description = "The Government Gateway ID for the newly created individual test user.")]
    public string GatewayId { get; set; } = string.Empty;

    /// <summary>
    /// The Government Gateway password for the newly created individual test user.
    /// This password is used for login purposes in the Government Gateway.
    /// **Important:** Store this securely and temporarily, as it's only needed for initial setup/testing.
    /// </summary>
    /// <example>password123</example>
    [SwaggerSchema(Description = "The Government Gateway password for the newly created individual test user.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The full name associated with the Government Gateway user.
    /// </summary>
    /// <example>John Doe</example>
    [SwaggerSchema(Description = "The Government Gateway user's full name.")]
    public string UserFullName { get; set; } = string.Empty;

    /// <summary>
    /// The email address associated with the Government Gateway user.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [SwaggerSchema(Description = "The Government Gateway user's email address.")]
    public string EmailAddress { get; set; } = string.Empty;
}