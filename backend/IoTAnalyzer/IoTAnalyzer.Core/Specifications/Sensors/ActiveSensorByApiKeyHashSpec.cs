using Ardalis.Specification;
using IoTAnalyzer.Core.Entities;

namespace IoTAnalyzer.Core.Specifications.Sensors;

public sealed class ActiveSensorByApiKeyHashSpec : SingleResultSpecification<Sensor>
{
    public ActiveSensorByApiKeyHashSpec(string apiKeyHash)
    {
        Query.Where(s => s.ApiKeyHash == apiKeyHash && s.UnregisteredAt == null);
    }
}
