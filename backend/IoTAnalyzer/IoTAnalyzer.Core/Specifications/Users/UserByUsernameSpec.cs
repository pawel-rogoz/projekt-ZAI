using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.Users;

public sealed class UserByUsernameSpec : SingleResultSpecification<User>
{
    public UserByUsernameSpec(string username)
    {
        Query.Where(u => u.Username == username);
    }
}
