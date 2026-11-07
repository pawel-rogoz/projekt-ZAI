using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.AuthSessions;

public sealed class ExpiredSessionsSpec : Specification<AuthSession>
{
    public ExpiredSessionsSpec(DateTime now)
    {
        Query.Where(s => s.ExpiresAt <= now);
    }
}
