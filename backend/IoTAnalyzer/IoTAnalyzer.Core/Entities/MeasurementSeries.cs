using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace IoTAnalyzer.Core.Entities;

public class MeasurementSeries : EntityBase, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public double MinValue { get; private set; }
    public double MaxValue { get; private set; }
    public string Color { get; private set; } = string.Empty;
    public string? Icon { get; private set; }
    public string? Unit { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private MeasurementSeries()
    {
    }

    public MeasurementSeries(string name, double minValue, double maxValue, string color, string? icon, string? unit)
    {
        Update(name, minValue, maxValue, color, icon, unit);
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, double minValue, double maxValue, string color, string? icon, string? unit)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(color);
        Guard.Against.Expression(_ => minValue >= maxValue, minValue, "Wartość minimalna musi być mniejsza od maksymalnej.");

        Name = name.Trim();
        MinValue = minValue;
        MaxValue = maxValue;
        Color = color.ToLowerInvariant();
        Icon = string.IsNullOrWhiteSpace(icon) ? null : icon.Trim();
        Unit = string.IsNullOrWhiteSpace(unit) ? null : unit.Trim();
    }

    public bool Accepts(double value) => value >= MinValue && value <= MaxValue;
}
