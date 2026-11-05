using Ardalis.SharedKernel;

namespace IoTAnalyzer.Core.Entities;

public class Measurement : EntityBase<long>, IAggregateRoot
{
    public int SeriesId { get; private set; }
    public int? SensorId { get; private set; }
    public double Value { get; private set; }
    public DateTime Timestamp { get; private set; }
    public DateTime ReceivedAt { get; private set; }

    private Measurement()
    {
    }

    public Measurement(int seriesId, int sensorId, double value, DateTime timestamp, DateTime receivedAt)
    {
        SeriesId = seriesId;
        SensorId = sensorId;
        Value = value;
        Timestamp = timestamp;
        ReceivedAt = receivedAt;
    }
}
