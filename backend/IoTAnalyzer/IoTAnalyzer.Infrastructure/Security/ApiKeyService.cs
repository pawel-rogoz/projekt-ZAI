using System.Security.Cryptography;
using System.Text;
using IoTAnalyzer.Core.Interfaces;

namespace IoTAnalyzer.Infrastructure.Security;

public class ApiKeyService : IApiKeyService
{
    private const string Prefix = "iot_";
    private const int KeySizeInBytes = 32;

    public string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(KeySizeInBytes);
        return Prefix + Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public string Hash(string apiKey) =>
        Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(apiKey)));
}
