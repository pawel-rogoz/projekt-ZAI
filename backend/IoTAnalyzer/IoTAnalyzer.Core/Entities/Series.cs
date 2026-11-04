namespace IoTAnalyzer.Core.Entities;

public class Series
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    public required string Color { get; set; }
    public string? Icon { get; set; }
    public string? Unit { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
    public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();

    public bool Accepts(double value) => value >= MinValue && value <= MaxValue;
}
