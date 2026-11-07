namespace IoTAnalyzer.UseCases.Sensors.RegisterSensor;

public record RegisteredSensorDto(int Id, string Name, int SeriesId, DateTime CreatedAt, DateTime? LastMeasurementAt, string ApiKey);
