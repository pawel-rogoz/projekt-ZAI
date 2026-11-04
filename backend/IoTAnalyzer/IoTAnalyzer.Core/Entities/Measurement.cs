namespace IoTAnalyzer.Core.Entities;

public class Measurement
{
    public long Id { get; set; }
    public int SeriesId { get; set; }
    public Series Series { get; set; } = null!;
    public int? SensorId { get; set; }
    public Sensor? Sensor { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime ReceivedAt { get; set; }
}
