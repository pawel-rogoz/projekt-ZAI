using System.Security.Cryptography;
using System.Text;
using IoTAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace IoTAnalyzer.Infrastructure.Security;

public class AccessTokenService(IOptions<AuthOptions> options) : IAccessTokenService
{
    private const int TokenSizeInBytes = 32;

    public TimeSpan Lifetime { get; } = TimeSpan.FromMinutes(options.Value.TokenLifetimeMinutes);

    public string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(TokenSizeInBytes);
        return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public string Hash(string accessToken) =>
        Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(accessToken)));
}
