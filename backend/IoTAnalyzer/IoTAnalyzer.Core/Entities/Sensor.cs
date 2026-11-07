using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace IoTAnalyzer.Core.Entities;

public class Sensor : EntityBase, IAggregateRoot
{
    public int SeriesId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string ApiKeyHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastMeasurementAt { get; private set; }
    public DateTime? UnregisteredAt { get; private set; }

    public bool IsActive => UnregisteredAt is null;

    private Sensor()
    {
    }

    public Sensor(string name, int seriesId, string apiKeyHash)
    {
        Name = Guard.Against.NullOrWhiteSpace(name).Trim();
        SeriesId = Guard.Against.NegativeOrZero(seriesId);
        ApiKeyHash = Guard.Against.NullOrWhiteSpace(apiKeyHash);
        CreatedAt = DateTime.UtcNow;
    }

    public void Unregister()
    {
        UnregisteredAt ??= DateTime.UtcNow;
    }

    public void RegisterMeasurement(DateTime receivedAt)
    {
        LastMeasurementAt = receivedAt;
    }
}
