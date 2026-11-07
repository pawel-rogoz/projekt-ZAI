using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Specifications.Measurements;
using Mediator;

namespace IoTAnalyzer.UseCases.Measurements.ListMeasurements;

public class ListMeasurementsHandler(IReadRepository<Measurement> repository)
    : IQueryHandler<ListMeasurementsQuery, Result<List<MeasurementDto>>>
{
    public async ValueTask<Result<List<MeasurementDto>>> Handle(ListMeasurementsQuery query, CancellationToken cancellationToken)
    {
        if (query.From > query.To)
            return Result.Invalid(new ValidationError { Identifier = "from", ErrorMessage = "Początek przedziału nie może być późniejszy niż koniec." });

        var spec = new MeasurementsFilterSpec(query.SeriesIds, query.From, query.To, query.Descending, query.Limit);
        var measurements = await repository.ListAsync(spec, cancellationToken);
        return measurements.Select(MeasurementDto.FromEntity).ToList();
    }
}
