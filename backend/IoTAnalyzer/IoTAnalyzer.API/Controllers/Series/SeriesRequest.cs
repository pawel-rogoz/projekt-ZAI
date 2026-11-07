using System.ComponentModel.DataAnnotations;

namespace IoTAnalyzer.API.Controllers.Series;

public class SeriesRequest
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public double? MinValue { get; set; }

    [Required]
    public double? MaxValue { get; set; }

    [Required]
    public string? Color { get; set; }

    public string? Icon { get; set; }

    public string? Unit { get; set; }
}
