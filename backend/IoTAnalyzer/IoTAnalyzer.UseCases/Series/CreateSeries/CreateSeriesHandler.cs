using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.CreateSeries;

public class CreateSeriesHandler(IRepository<MeasurementSeries> repository)
    : ICommandHandler<CreateSeriesCommand, Result<SeriesDto>>
{
    public async ValueTask<Result<SeriesDto>> Handle(CreateSeriesCommand command, CancellationToken cancellationToken)
    {
        var errors = SeriesValidator.Validate(command.Name, command.MinValue, command.MaxValue, command.Color, command.Icon, command.Unit);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var series = new MeasurementSeries(command.Name, command.MinValue, command.MaxValue, command.Color, command.Icon, command.Unit);
        await repository.AddAsync(series, cancellationToken);

        return SeriesDto.FromEntity(series);
    }
}
