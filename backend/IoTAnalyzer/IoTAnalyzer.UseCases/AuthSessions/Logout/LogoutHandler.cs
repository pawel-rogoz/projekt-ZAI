using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using Mediator;

namespace IoTAnalyzer.UseCases.AuthSessions.Logout;

public class LogoutHandler(IRepository<AuthSession> repository) : ICommandHandler<LogoutCommand, Result>
{
    public async ValueTask<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var session = await repository.GetByIdAsync(command.SessionId, cancellationToken);
        if (session is not null)
            await repository.DeleteAsync(session, cancellationToken);

        return Result.Success();
    }
}
