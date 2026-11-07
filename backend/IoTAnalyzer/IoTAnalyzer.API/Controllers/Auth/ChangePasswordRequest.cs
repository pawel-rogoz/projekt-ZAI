using System.ComponentModel.DataAnnotations;

namespace IoTAnalyzer.API.Controllers.Auth;

public class ChangePasswordRequest
{
    [Required]
    public string? CurrentPassword { get; set; }

    [Required]
    public string? NewPassword { get; set; }
}
