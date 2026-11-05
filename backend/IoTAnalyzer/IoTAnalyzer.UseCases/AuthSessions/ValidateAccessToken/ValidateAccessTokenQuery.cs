using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.AuthSessions.ValidateAccessToken;

public record ValidateAccessTokenQuery(string AccessToken) : IQuery<Result<AuthSessionDto>>;
