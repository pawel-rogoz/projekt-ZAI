using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.GetSeriesById;

public record GetSeriesByIdQuery(int SeriesId) : IQuery<Result<SeriesDto>>;
