using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.ListSensors;

public record ListSensorsQuery : IQuery<Result<List<SensorDto>>>;
