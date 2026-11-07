using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.AuthSessions.Logout;

public record LogoutCommand(long SessionId) : ICommand<Result>;
