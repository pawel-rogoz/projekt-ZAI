namespace IoTAnalyzer.Infrastructure.Security;

public class AuthOptions
{
    public const string SectionName = "Auth";

    public int TokenLifetimeMinutes { get; set; } = 60;
    public int BcryptWorkFactor { get; set; } = 12;
    public string AdminUsername { get; set; } = "admin";
    public string? AdminPassword { get; set; }
}
