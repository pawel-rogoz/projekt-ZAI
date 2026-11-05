using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.Measurements;

public sealed class MeasurementsOutsideRangeSpec : Specification<Measurement>
{
    public MeasurementsOutsideRangeSpec(int seriesId, double minValue, double maxValue)
    {
        Query.Where(m => m.SeriesId == seriesId && (m.Value < minValue || m.Value > maxValue));
    }
}
