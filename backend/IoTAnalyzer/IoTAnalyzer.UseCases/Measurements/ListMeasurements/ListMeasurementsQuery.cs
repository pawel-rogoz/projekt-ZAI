using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Measurements.ListMeasurements;

public record ListMeasurementsQuery(
    IReadOnlyCollection<int> SeriesIds,
    DateTime? From,
    DateTime? To,
    bool Descending,
    int Limit) : IQuery<Result<List<MeasurementDto>>>
{
    public const int DefaultLimit = 1000;
    public const int MaxLimit = 10000;
}
