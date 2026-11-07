using System.Security.Claims;
using System.Text.Encodings.Web;
using IoTAnalyzer.UseCases.AuthSessions.ValidateAccessToken;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace IoTAnalyzer.API.Authentication;

public class AccessTokenAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IMediator mediator)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "AccessToken";
    private const string BearerPrefix = "Bearer ";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? header = Request.Headers.Authorization;
        if (string.IsNullOrEmpty(header) || !header.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.NoResult();

        var token = header[BearerPrefix.Length..].Trim();
        if (token.Length is 0 or > 512)
            return AuthenticateResult.Fail("Niepoprawny token.");

        var result = await mediator.Send(new ValidateAccessTokenQuery(token), Context.RequestAborted);
        if (!result.IsSuccess)
            return AuthenticateResult.Fail("Token jest nieważny lub wygasł.");

        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, result.Value.UserId.ToString()),
            new Claim(AuthClaims.SessionId, result.Value.SessionId.ToString()),
        ], SchemeName);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        Response.Headers.WWWAuthenticate = "Bearer";
        return Task.CompletedTask;
    }
}
