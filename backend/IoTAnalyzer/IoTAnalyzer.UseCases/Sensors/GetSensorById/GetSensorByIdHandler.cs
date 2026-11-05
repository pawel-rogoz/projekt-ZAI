using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Specifications.Sensors;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.GetSensorById;

public class GetSensorByIdHandler(IReadRepository<Sensor> repository)
    : IQueryHandler<GetSensorByIdQuery, Result<SensorDto>>
{
    public async ValueTask<Result<SensorDto>> Handle(GetSensorByIdQuery query, CancellationToken cancellationToken)
    {
        var sensor = await repository.FirstOrDefaultAsync(new ActiveSensorByIdSpec(query.SensorId), cancellationToken);
        if (sensor is null)
            return Result.NotFound($"Czujnik o identyfikatorze {query.SensorId} nie istnieje.");

        return SensorDto.FromEntity(sensor);
    }
}
