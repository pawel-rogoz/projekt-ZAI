using System.ComponentModel.DataAnnotations;

namespace IoTAnalyzer.API.Controllers.Auth;

public class LoginRequest
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}
