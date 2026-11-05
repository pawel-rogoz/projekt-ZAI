using Microsoft.AspNetCore.Mvc;

namespace IoTAnalyzer.API.Controllers.Health;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "ok" });
}
