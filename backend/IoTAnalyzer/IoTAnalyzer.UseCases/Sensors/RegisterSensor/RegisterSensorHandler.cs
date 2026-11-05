using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Interfaces;
using Mediator;

namespace IoTAnalyzer.UseCases.Sensors.RegisterSensor;

public class RegisterSensorHandler(
    IRepository<Sensor> sensorRepository,
    IReadRepository<MeasurementSeries> seriesRepository,
    IApiKeyService apiKeyService)
    : ICommandHandler<RegisterSensorCommand, Result<RegisteredSensorDto>>
{
    public const int NameMaxLength = 100;

    public async ValueTask<Result<RegisteredSensorDto>> Handle(RegisterSensorCommand command, CancellationToken cancellationToken)
    {
        var name = command.Name?.Trim() ?? string.Empty;
        if (name.Length is 0 or > NameMaxLength)
            return Result.Invalid(new ValidationError { Identifier = "name", ErrorMessage = $"Nazwa musi mieć od 1 do {NameMaxLength} znaków." });

        var series = await seriesRepository.GetByIdAsync(command.SeriesId, cancellationToken);
        if (series is null)
            return Result.Invalid(new ValidationError { Identifier = "seriesId", ErrorMessage = $"Seria o identyfikatorze {command.SeriesId} nie istnieje." });

        var apiKey = apiKeyService.Generate();
        var sensor = new Sensor(name, series.Id, apiKeyService.Hash(apiKey));
        await sensorRepository.AddAsync(sensor, cancellationToken);

        return new RegisteredSensorDto(sensor.Id, sensor.Name, sensor.SeriesId, sensor.CreatedAt, sensor.LastMeasurementAt, apiKey);
    }
}
