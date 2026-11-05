using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.DeleteSeries;

public record DeleteSeriesCommand(int SeriesId) : ICommand<Result>;
