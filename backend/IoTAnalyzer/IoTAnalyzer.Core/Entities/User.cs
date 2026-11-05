using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace IoTAnalyzer.Core.Entities;

public class User : EntityBase, IAggregateRoot
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime PasswordChangedAt { get; private set; }

    private User()
    {
    }

    public User(string username, string passwordHash)
    {
        Username = Guard.Against.NullOrWhiteSpace(username).Trim();
        PasswordHash = Guard.Against.NullOrWhiteSpace(passwordHash);
        CreatedAt = DateTime.UtcNow;
        PasswordChangedAt = CreatedAt;
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = Guard.Against.NullOrWhiteSpace(passwordHash);
        PasswordChangedAt = DateTime.UtcNow;
    }
}
