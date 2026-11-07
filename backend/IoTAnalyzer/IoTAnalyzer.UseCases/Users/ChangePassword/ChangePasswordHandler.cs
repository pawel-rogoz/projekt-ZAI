using System.Text;
using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Interfaces;
using IoTAnalyzer.Core.Specifications.AuthSessions;
using Mediator;

namespace IoTAnalyzer.UseCases.Users.ChangePassword;

public class ChangePasswordHandler(
    IRepository<User> userRepository,
    IRepository<AuthSession> sessionRepository,
    IPasswordHasher passwordHasher)
    : ICommandHandler<ChangePasswordCommand, Result>
{
    public const int MinPasswordLength = 8;
    public const int MaxPasswordBytes = 72;

    public async ValueTask<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();
        if (command.NewPassword.Length < MinPasswordLength)
            errors.Add(new ValidationError { Identifier = "newPassword", ErrorMessage = $"Hasło musi mieć co najmniej {MinPasswordLength} znaków." });
        if (Encoding.UTF8.GetByteCount(command.NewPassword) > MaxPasswordBytes)
            errors.Add(new ValidationError { Identifier = "newPassword", ErrorMessage = $"Hasło może mieć najwyżej {MaxPasswordBytes} bajty." });
        if (command.NewPassword == command.CurrentPassword)
            errors.Add(new ValidationError { Identifier = "newPassword", ErrorMessage = "Nowe hasło musi różnić się od bieżącego." });
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result.Unauthorized();

        if (!passwordHasher.Verify(command.CurrentPassword, user.PasswordHash))
            return Result.Forbidden("Podane bieżące hasło jest niepoprawne.");

        user.ChangePassword(passwordHasher.Hash(command.NewPassword));
        await userRepository.UpdateAsync(user, cancellationToken);

        await sessionRepository.DeleteRangeAsync(new OtherUserSessionsSpec(user.Id, command.SessionId), cancellationToken);

        return Result.Success();
    }
}
