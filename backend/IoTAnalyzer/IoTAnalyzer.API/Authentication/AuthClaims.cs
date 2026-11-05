using System.Security.Claims;

namespace IoTAnalyzer.API.Authentication;

public static class AuthClaims
{
    public const string SessionId = "sid";

    public static int GetUserId(this ClaimsPrincipal principal) =>
        int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static long GetSessionId(this ClaimsPrincipal principal) =>
        long.Parse(principal.FindFirstValue(SessionId)!);
}
