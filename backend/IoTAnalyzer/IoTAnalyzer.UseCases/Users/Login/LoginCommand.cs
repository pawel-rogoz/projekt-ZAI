using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Users.Login;

public record LoginCommand(string Username, string Password) : ICommand<Result<LoginResultDto>>;
