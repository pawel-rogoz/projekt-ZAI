using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.AuthSessions;

public sealed class OtherUserSessionsSpec : Specification<AuthSession>
{
    public OtherUserSessionsSpec(int userId, long currentSessionId)
    {
        Query.Where(s => s.UserId == userId && s.Id != currentSessionId);
    }
}
