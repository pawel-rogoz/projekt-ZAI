using Ardalis.Specification;

namespace IoTAnalyzer.Core.Specifications.Series;

public sealed class SeriesListSpec : Specification<Entities.MeasurementSeries>
{
    public SeriesListSpec()
    {
        Query.AsNoTracking().OrderBy(s => s.Id);
    }
}
