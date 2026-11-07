using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.ListSeries;

public record ListSeriesQuery : IQuery<Result<List<SeriesDto>>>;
