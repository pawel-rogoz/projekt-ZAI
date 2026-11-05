using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.CreateSeries;

public record CreateSeriesCommand(string Name, double MinValue, double MaxValue, string Color, string? Icon, string? Unit)
    : ICommand<Result<SeriesDto>>;
