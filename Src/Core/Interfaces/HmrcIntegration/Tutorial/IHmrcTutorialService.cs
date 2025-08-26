namespace Core.Interfaces.HmrcIntegration.Tutorial;

public interface IHmrcTutorialService
{
    Task<string> GetHelloWorldAsync();

    Task<string> GetHelloApplicationAsync();

    Task<string> GetHelloUserAsync(string userId);
}