using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Measurements.GetMeasurementById;

public record GetMeasurementByIdQuery(long MeasurementId) : IQuery<Result<MeasurementDto>>;
