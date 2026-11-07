using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using IoTAnalyzer.API.Authentication;
using IoTAnalyzer.UseCases.AuthSessions.Logout;
using IoTAnalyzer.UseCases.Users.ChangePassword;
using IoTAnalyzer.UseCases.Users.Login;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace IoTAnalyzer.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
[TranslateResultToActionResult]
public class AuthController(IMediator mediator) : ControllerBase
{
    public const string LoginRateLimitPolicy = "login";

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting(LoginRateLimitPolicy)]
    [ExpectedFailures(ResultStatus.Unauthorized)]
    public async Task<Result<LoginResultDto>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        Response.Headers.CacheControl = "no-store";
        return await mediator.Send(new LoginCommand(request.Username!, request.Password!), cancellationToken);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<Result> Logout(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LogoutCommand(User.GetSessionId()), cancellationToken);
        return result.IsSuccess ? Result.NoContent() : result;
    }

    [HttpPut("password")]
    [Authorize]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Forbidden)]
    public async Task<Result> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(User.GetUserId(), User.GetSessionId(), request.CurrentPassword!, request.NewPassword!);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Result.NoContent() : result;
    }
}
