using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.UseCases.Series;

public record SeriesDto(int Id, string Name, double MinValue, double MaxValue, string Color, string? Icon, string? Unit)
{
    public static SeriesDto FromEntity(MeasurementSeries series) =>
        new(series.Id, series.Name, series.MinValue, series.MaxValue, series.Color, series.Icon, series.Unit);
}
