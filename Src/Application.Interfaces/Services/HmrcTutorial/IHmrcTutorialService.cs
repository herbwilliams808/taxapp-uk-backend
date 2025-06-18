using System.Threading.Tasks;

namespace Application.Interfaces.Services.HmrcTutorial;

public interface IHmrcTutorialService
{
    /// <summary>
    /// Calls the HMRC "Hello World" test endpoint and returns its response.
    /// Does NOT require authentication.
    /// </summary>
    /// <returns>The string response from the Hello World API.</returns>
    Task<string> GetHelloWorldAsync();

    /// <summary>
    /// Calls the HMRC "Hello Application" test endpoint and returns its response.
    /// Requires Application (OAuth 2.0 access_token) authentication.
    /// </summary>
    /// <returns>The string response from the Hello Application API.</returns>
    Task<string> GetHelloApplicationAsync();

    /// <summary>
    /// Calls the HMRC "Hello User" test endpoint and returns its response.
    /// Requires User (OAuth 2.0 access_token) authentication.
    /// </summary>
    /// <param name="userId">The unique identifier for the user whose token should be used.</param>
    /// <returns>The string response from the Hello User API.</returns>
    Task<string> GetHelloUserAsync(string userId); // New method
}