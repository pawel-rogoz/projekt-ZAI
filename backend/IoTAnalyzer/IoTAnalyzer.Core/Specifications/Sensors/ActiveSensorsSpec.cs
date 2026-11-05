using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.Sensors;

public sealed class ActiveSensorsSpec : Specification<Sensor>
{
    public ActiveSensorsSpec()
    {
        Query.AsNoTracking().Where(s => s.UnregisteredAt == null).OrderBy(s => s.Id);
    }
}
