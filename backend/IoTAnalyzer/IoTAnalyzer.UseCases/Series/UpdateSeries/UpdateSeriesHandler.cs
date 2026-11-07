using System.Globalization;
using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Specifications.Measurements;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.UpdateSeries;

public class UpdateSeriesHandler(
    IRepository<MeasurementSeries> seriesRepository,
    IReadRepository<Measurement> measurementRepository)
    : ICommandHandler<UpdateSeriesCommand, Result<SeriesDto>>
{
    public async ValueTask<Result<SeriesDto>> Handle(UpdateSeriesCommand command, CancellationToken cancellationToken)
    {
        var errors = SeriesValidator.Validate(command.Name, command.MinValue, command.MaxValue, command.Color, command.Icon, command.Unit);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var series = await seriesRepository.GetByIdAsync(command.SeriesId, cancellationToken);
        if (series is null)
            return Result.NotFound($"Seria o identyfikatorze {command.SeriesId} nie istnieje.");

        var outside = new MeasurementsOutsideRangeSpec(series.Id, command.MinValue, command.MaxValue);
        if (await measurementRepository.AnyAsync(outside, cancellationToken))
            return Result.Conflict(string.Create(CultureInfo.InvariantCulture,
                $"Przedział [{command.MinValue}, {command.MaxValue}] wyklucza wyniki zapisane już w serii."));

        series.Update(command.Name, command.MinValue, command.MaxValue, command.Color, command.Icon, command.Unit);
        await seriesRepository.UpdateAsync(series, cancellationToken);

        return SeriesDto.FromEntity(series);
    }
}
