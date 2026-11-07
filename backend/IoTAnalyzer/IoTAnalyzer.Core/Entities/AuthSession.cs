using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace IoTAnalyzer.Core.Entities;

public class AuthSession : EntityBase<long>, IAggregateRoot
{
    public int UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private AuthSession()
    {
    }

    public AuthSession(int userId, string tokenHash, TimeSpan lifetime)
    {
        UserId = userId;
        TokenHash = Guard.Against.NullOrWhiteSpace(tokenHash);
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt + lifetime;
    }

    public bool IsExpired(DateTime now) => ExpiresAt <= now;
}
