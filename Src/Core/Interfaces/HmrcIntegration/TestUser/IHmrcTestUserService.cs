using Core.Models.HmrcIntegration.TestUser;

namespace Core.Interfaces.HmrcIntegration.TestUser;

public interface IHmrcTestUserService
{
    Task<CreateIndividualTestUserResponse> CreateIndividualTestUserAsync(CreateIndividualTestUserRequest request);
}