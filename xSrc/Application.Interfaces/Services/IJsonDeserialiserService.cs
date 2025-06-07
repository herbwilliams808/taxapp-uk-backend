namespace Application.Interfaces.Services;

public interface IJsonDeserialiserService
{
    /// <summary>
    /// deserialises JSON content into a strongly typed object.
    /// </summary>
    /// <typeparam name="T">The type to which the JSON should be deserialised.</typeparam>
    /// <param name="jsonContent">The JSON string to deserialise.</param>
    /// <returns>The deserialised object of type T.</returns>
    T Deserialise<T>(string jsonContent);

    /// <summary>
    /// deserialises JSON content into a dynamic object.
    /// </summary>
    /// <param name="jsonContent">The JSON string to deserialise.</param>
    /// <returns>The deserialised dynamic object.</returns>
    object? DeserialiseDynamic(string jsonContent);
}