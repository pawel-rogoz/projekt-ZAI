using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.UnregisterSensor;

public record UnregisterSensorCommand(int SensorId) : ICommand<Result>;
