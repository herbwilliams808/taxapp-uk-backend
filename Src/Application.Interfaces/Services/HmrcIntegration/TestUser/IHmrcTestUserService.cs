using Shared.Models.HmrcIntegration.TestUser;

namespace Application.Interfaces.Services.HmrcIntegration.TestUser;

public interface IHmrcTestUserService
{
    /// <summary>
    /// Creates a new individual test user with specified service enrolments using the HMRC Test User API.
    /// </summary>
    /// <param name="request">The request containing the list of service names for enrolment.</param>
    /// <returns>A <see cref="CreateIndividualTestUserResponse"/> object containing details of the created user.</returns>
    Task<CreateIndividualTestUserResponse> CreateIndividualTestUserAsync(CreateIndividualTestUserRequest request);
}