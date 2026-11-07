using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.Measurements;

public sealed class MeasurementsFilterSpec : Specification<Measurement>
{
    public MeasurementsFilterSpec(IReadOnlyCollection<int> seriesIds, DateTime? from, DateTime? to, bool descending, int limit)
    {
        Query.AsNoTracking();

        if (seriesIds.Count > 0)
            Query.Where(m => seriesIds.Contains(m.SeriesId));
        if (from is not null)
            Query.Where(m => m.Timestamp >= from);
        if (to is not null)
            Query.Where(m => m.Timestamp <= to);

        if (descending)
            Query.OrderByDescending(m => m.Timestamp).ThenByDescending(m => m.Id);
        else
            Query.OrderBy(m => m.Timestamp).ThenBy(m => m.Id);

        Query.Take(limit);
    }
}
