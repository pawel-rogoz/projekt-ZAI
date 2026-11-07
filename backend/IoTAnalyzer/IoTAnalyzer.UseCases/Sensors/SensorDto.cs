using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.UseCases.Sensors;

public record SensorDto(int Id, string Name, int SeriesId, DateTime CreatedAt, DateTime? LastMeasurementAt)
{
    public static SensorDto FromEntity(Sensor sensor) =>
        new(sensor.Id, sensor.Name, sensor.SeriesId, sensor.CreatedAt, sensor.LastMeasurementAt);
}
