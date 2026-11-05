using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Interfaces;
using IoTAnalyzer.Core.Specifications.AuthSessions;
using Mediator;

namespace IoTAnalyzer.UseCases.AuthSessions.ValidateAccessToken;

public class ValidateAccessTokenHandler(IReadRepository<AuthSession> repository, IAccessTokenService accessTokenService)
    : IQueryHandler<ValidateAccessTokenQuery, Result<AuthSessionDto>>
{
    public async ValueTask<Result<AuthSessionDto>> Handle(ValidateAccessTokenQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.AccessToken))
            return Result.Unauthorized();

        var hash = accessTokenService.Hash(query.AccessToken);
        var session = await repository.FirstOrDefaultAsync(new ActiveSessionByTokenHashSpec(hash, DateTime.UtcNow), cancellationToken);
        if (session is null)
            return Result.Unauthorized();

        return new AuthSessionDto(session.Id, session.UserId, session.ExpiresAt);
    }
}
