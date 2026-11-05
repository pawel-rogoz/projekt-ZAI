using Ardalis.Result;
using Mediator;

namespace IoTAnalyzer.UseCases.Measurements.CreateMeasurement;

public record CreateMeasurementCommand(string? ApiKey, double Value, DateTime? Timestamp) : ICommand<Result<MeasurementDto>>;
