using Ardalis.Result;
using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Entities;
using Mediator;

namespace IoTAnalyzer.UseCases.Series.DeleteSeries;

public class DeleteSeriesHandler(IRepository<MeasurementSeries> repository)
    : ICommandHandler<DeleteSeriesCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteSeriesCommand command, CancellationToken cancellationToken)
    {
        var series = await repository.GetByIdAsync(command.SeriesId, cancellationToken);
        if (series is null)
            return Result.NotFound($"Seria o identyfikatorze {command.SeriesId} nie istnieje.");

        await repository.DeleteAsync(series, cancellationToken);
        return Result.Success();
    }
}
