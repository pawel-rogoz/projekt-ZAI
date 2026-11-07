using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.GetSeriesById;

public class GetSeriesByIdHandler(IReadRepository<MeasurementSeries> repository)
    : IQueryHandler<GetSeriesByIdQuery, Result<SeriesDto>>
{
    public async ValueTask<Result<SeriesDto>> Handle(GetSeriesByIdQuery query, CancellationToken cancellationToken)
    {
        var series = await repository.GetByIdAsync(query.SeriesId, cancellationToken);
        if (series is null)
            return Result.NotFound($"Seria o identyfikatorze {query.SeriesId} nie istnieje.");

        return SeriesDto.FromEntity(series);
    }
}
