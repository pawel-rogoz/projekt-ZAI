using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.Sensors;

public sealed class ActiveSensorByIdSpec : SingleResultSpecification<Sensor>
{
    public ActiveSensorByIdSpec(int sensorId)
    {
        Query.Where(s => s.Id == sensorId && s.UnregisteredAt == null);
    }
}
