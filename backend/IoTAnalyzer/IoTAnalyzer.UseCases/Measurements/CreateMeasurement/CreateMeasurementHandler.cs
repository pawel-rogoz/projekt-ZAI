using System.Globalization;
using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Interfaces;
using IoTAnalyzer.Core.Specifications.Sensors;
using Mediator;
using Microsoft.Extensions.Logging;

namespace IoTAnalyzer.UseCases.Measurements.CreateMeasurement;

public class CreateMeasurementHandler(
    IRepository<Measurement> measurementRepository,
    IRepository<Sensor> sensorRepository,
    IReadRepository<MeasurementSeries> seriesRepository,
    IApiKeyService apiKeyService,
    ILogger<CreateMeasurementHandler> logger)
    : ICommandHandler<CreateMeasurementCommand, Result<MeasurementDto>>
{
    private static readonly TimeSpan MaxClockSkew = TimeSpan.FromMinutes(5);

    public async ValueTask<Result<MeasurementDto>> Handle(CreateMeasurementCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.ApiKey))
            return Result.Unauthorized("Brak klucza API czujnika.");

        var hash = apiKeyService.Hash(command.ApiKey.Trim());
        var sensor = await sensorRepository.FirstOrDefaultAsync(new ActiveSensorByApiKeyHashSpec(hash), cancellationToken);
        if (sensor is null)
            return Result.Unauthorized("Klucz API jest nieważny lub czujnik został wyrejestrowany.");

        var series = await seriesRepository.GetByIdAsync(sensor.SeriesId, cancellationToken);
        if (series is null)
            return Result.Unauthorized("Seria czujnika nie istnieje.");

        var now = DateTime.UtcNow;
        var timestamp = command.Timestamp ?? now;

        if (timestamp > now + MaxClockSkew)
        {
            logger.LogWarning("Odrzucono wynik czujnika {SensorId} (seria {SeriesId}): znacznik czasu {Timestamp:o} z przyszłości",
                sensor.Id, series.Id, timestamp);
            return Result.Invalid(new ValidationError
            {
                Identifier = "timestamp",
                ErrorMessage = "Znacznik czasu nie może być późniejszy niż 5 minut od bieżącego czasu serwera.",
            });
        }

        if (!series.Accepts(command.Value))
        {
            logger.LogWarning("Odrzucono wynik czujnika {SensorId} (seria {SeriesId}): wartość {Value} spoza przedziału [{Min}, {Max}]",
                sensor.Id, series.Id, command.Value, series.MinValue, series.MaxValue);
            return Result.Invalid(new ValidationError
            {
                Identifier = "value",
                ErrorMessage = string.Create(CultureInfo.InvariantCulture,
                    $"Wartość {command.Value} jest spoza dopuszczalnego przedziału serii [{series.MinValue}, {series.MaxValue}]."),
            });
        }

        var measurement = new Measurement(series.Id, sensor.Id, command.Value, timestamp, now);
        await measurementRepository.AddAsync(measurement, cancellationToken);

        sensor.RegisterMeasurement(now);
        await sensorRepository.UpdateAsync(sensor, cancellationToken);

        return MeasurementDto.FromEntity(measurement);
    }
}
