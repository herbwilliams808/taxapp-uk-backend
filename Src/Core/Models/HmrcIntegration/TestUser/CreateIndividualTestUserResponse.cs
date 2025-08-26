using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.HmrcIntegration.TestUser;

[SwaggerSchema(Description = "Represents the response received after successfully creating an individual test user.")]
public class CreateIndividualTestUserResponse
{
    [SwaggerSchema(Description = "The MTD ID for the newly created individual test user.")]
    public string MtdId { get; set; } = string.Empty;

    [SwaggerSchema(Description = "The Government Gateway ID for the newly created individual test user.")]
    public string GatewayId { get; set; } = string.Empty;

    [SwaggerSchema(Description = "The Government Gateway password for the newly created individual test user.")]
    public string Password { get; set; } = string.Empty;

    [SwaggerSchema(Description = "The Government Gateway user's full name.")]
    public string UserFullName { get; set; } = string.Empty;

    [SwaggerSchema(Description = "The Government Gateway user's email address.")]
    public string EmailAddress { get; set; } = string.Empty;
}