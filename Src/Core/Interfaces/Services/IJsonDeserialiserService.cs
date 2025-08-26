namespace Core.Interfaces.Services;

public interface IJsonDeserialiserService
{
    T Deserialise<T>(string jsonContent);

    object? DeserialiseDynamic(string jsonContent);
}