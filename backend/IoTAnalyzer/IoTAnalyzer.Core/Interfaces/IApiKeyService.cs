namespace IoTAnalyzer.Core.Interfaces;

public interface IApiKeyService
{
    string Generate();
    string Hash(string apiKey);
}
