using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using Mediator;

namespace IoTAnalyzer.UseCases.Measurements.GetMeasurementById;

public class GetMeasurementByIdHandler(IReadRepository<Measurement> repository)
    : IQueryHandler<GetMeasurementByIdQuery, Result<MeasurementDto>>
{
    public async ValueTask<Result<MeasurementDto>> Handle(GetMeasurementByIdQuery query, CancellationToken cancellationToken)
    {
        var measurement = await repository.GetByIdAsync(query.MeasurementId, cancellationToken);
        if (measurement is null)
            return Result.NotFound($"Wynik o identyfikatorze {query.MeasurementId} nie istnieje.");

        return MeasurementDto.FromEntity(measurement);
    }
}
