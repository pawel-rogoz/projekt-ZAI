using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Specifications.Sensors;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.ListSensors;

public class ListSensorsHandler(IReadRepository<Sensor> repository)
    : IQueryHandler<ListSensorsQuery, Result<List<SensorDto>>>
{
    public async ValueTask<Result<List<SensorDto>>> Handle(ListSensorsQuery query, CancellationToken cancellationToken)
    {
        var sensors = await repository.ListAsync(new ActiveSensorsSpec(), cancellationToken);
        return sensors.Select(SensorDto.FromEntity).ToList();
    }
}
