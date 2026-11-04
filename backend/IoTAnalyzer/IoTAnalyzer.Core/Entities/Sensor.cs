namespace IoTAnalyzer.Core.Entities;

public class Sensor
{
    public int Id { get; set; }
    public int SeriesId { get; set; }
    public Series Series { get; set; } = null!;
    public required string Name { get; set; }
    public required string ApiKeyHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMeasurementAt { get; set; }
    public DateTime? UnregisteredAt { get; set; }

    public bool IsActive => UnregisteredAt is null;

    public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
}
