using System.ComponentModel.DataAnnotations;

namespace IoTAnalyzer.API.Controllers.Measurements;

public class CreateMeasurementRequest
{
    [Required]
    public double? Value { get; set; }

    public DateTimeOffset? Timestamp { get; set; }
}
