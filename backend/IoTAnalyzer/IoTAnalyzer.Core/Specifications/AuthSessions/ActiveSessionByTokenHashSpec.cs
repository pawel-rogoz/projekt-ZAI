using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.AuthSessions;

public sealed class ActiveSessionByTokenHashSpec : SingleResultSpecification<AuthSession>
{
    public ActiveSessionByTokenHashSpec(string tokenHash, DateTime now)
    {
        Query.AsNoTracking().Where(s => s.TokenHash == tokenHash && s.ExpiresAt > now);
    }
}
