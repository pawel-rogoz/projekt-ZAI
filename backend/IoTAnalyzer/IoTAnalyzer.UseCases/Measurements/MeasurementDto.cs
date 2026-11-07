using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.UseCases.Measurements;

public record MeasurementDto(long Id, int SeriesId, int? SensorId, double Value, DateTime Timestamp)
{
    public static MeasurementDto FromEntity(Measurement measurement) =>
        new(measurement.Id, measurement.SeriesId, measurement.SensorId, measurement.Value, measurement.Timestamp);
}
