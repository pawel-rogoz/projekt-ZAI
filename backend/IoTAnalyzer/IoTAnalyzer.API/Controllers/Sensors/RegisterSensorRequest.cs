using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IoTAnalyzer.API.Controllers.Sensors;

public class RegisterSensorRequest
{
    [Required]
    public string? Name { get; set; }

    [Required]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? SeriesId { get; set; }
}
