namespace IoTAnalyzer.Core.Entities;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PasswordChangedAt { get; set; }

    public ICollection<AuthSession> Sessions { get; set; } = new List<AuthSession>();
}
