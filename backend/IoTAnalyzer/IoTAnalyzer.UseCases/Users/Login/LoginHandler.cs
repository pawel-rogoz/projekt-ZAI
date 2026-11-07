using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Interfaces;
using IoTAnalyzer.Core.Specifications.AuthSessions;
using IoTAnalyzer.Core.Specifications.Users;
using Mediator;

namespace IoTAnalyzer.UseCases.Users.Login;

public class LoginHandler(
    IReadRepository<User> userRepository,
    IRepository<AuthSession> sessionRepository,
    IPasswordHasher passwordHasher,
    IAccessTokenService accessTokenService)
    : ICommandHandler<LoginCommand, Result<LoginResultDto>>
{
    public async ValueTask<Result<LoginResultDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(new UserByUsernameSpec(command.Username.Trim()), cancellationToken);

        var passwordValid = passwordHasher.Verify(command.Password, user?.PasswordHash);
        if (user is null || !passwordValid)
            return Result.Unauthorized("Niepoprawna nazwa użytkownika lub hasło.");

        await sessionRepository.DeleteRangeAsync(new ExpiredSessionsSpec(DateTime.UtcNow), cancellationToken);

        var accessToken = accessTokenService.Generate();
        var session = new AuthSession(user.Id, accessTokenService.Hash(accessToken), accessTokenService.Lifetime);
        await sessionRepository.AddAsync(session, cancellationToken);

        return new LoginResultDto(accessToken, "Bearer", (int)accessTokenService.Lifetime.TotalSeconds);
    }
}
