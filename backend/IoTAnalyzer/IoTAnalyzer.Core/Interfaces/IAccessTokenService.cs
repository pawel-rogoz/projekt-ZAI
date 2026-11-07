namespace IoTAnalyzer.Core.Interfaces;

public interface IAccessTokenService
{
    TimeSpan Lifetime { get; }
    string Generate();
    string Hash(string accessToken);
}
