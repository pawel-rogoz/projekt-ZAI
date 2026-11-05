using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Users.ChangePassword;

public record ChangePasswordCommand(int UserId, long SessionId, string CurrentPassword, string NewPassword) : ICommand<Result>;
