using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.UpdateSeries;

public record UpdateSeriesCommand(int SeriesId, string Name, double MinValue, double MaxValue, string Color, string? Icon, string? Unit)
    : ICommand<Result<SeriesDto>>;
