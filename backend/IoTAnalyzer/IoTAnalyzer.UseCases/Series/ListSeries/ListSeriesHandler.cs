using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Specifications.Series;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.ListSeries;

public class ListSeriesHandler(IReadRepository<MeasurementSeries> repository)
    : IQueryHandler<ListSeriesQuery, Result<List<SeriesDto>>>
{
    public async ValueTask<Result<List<SeriesDto>>> Handle(ListSeriesQuery query, CancellationToken cancellationToken)
    {
        var series = await repository.ListAsync(new SeriesListSpec(), cancellationToken);
        return series.Select(SeriesDto.FromEntity).ToList();
    }
}
