using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.GetSensorById;

public record GetSensorByIdQuery(int SensorId) : IQuery<Result<SensorDto>>;
