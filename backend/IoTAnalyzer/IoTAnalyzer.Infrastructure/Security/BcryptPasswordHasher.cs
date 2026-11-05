using IoTAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace IoTAnalyzer.Infrastructure.Security;

public class BcryptPasswordHasher(IOptions<AuthOptions> options) : IPasswordHasher
{
    private readonly int _workFactor = options.Value.BcryptWorkFactor;
    private readonly Lazy<string> _dummyHash = new(() =>
        BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString(), options.Value.BcryptWorkFactor));

    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, _workFactor);

    public bool Verify(string password, string? passwordHash)
    {
        if (passwordHash is null)
        {
            BCrypt.Net.BCrypt.Verify(password, _dummyHash.Value);
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
    }
}
