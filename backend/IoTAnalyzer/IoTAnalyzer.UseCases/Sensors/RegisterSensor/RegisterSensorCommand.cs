using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.RegisterSensor;

public record RegisterSensorCommand(string Name, int SeriesId) : ICommand<Result<RegisteredSensorDto>>;
