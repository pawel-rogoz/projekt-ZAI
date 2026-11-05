using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Specifications.Sensors;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.UnregisterSensor;

public class UnregisterSensorHandler(IRepository<Sensor> repository)
    : ICommandHandler<UnregisterSensorCommand, Result>
{
    public async ValueTask<Result> Handle(UnregisterSensorCommand command, CancellationToken cancellationToken)
    {
        var sensor = await repository.FirstOrDefaultAsync(new ActiveSensorByIdSpec(command.SensorId), cancellationToken);
        if (sensor is null)
            return Result.NotFound($"Czujnik o identyfikatorze {command.SensorId} nie istnieje.");

        sensor.Unregister();
        await repository.UpdateAsync(sensor, cancellationToken);
        return Result.Success();
    }
}
