using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using IoTAnalyzer.UseCases.Series;
using IoTAnalyzer.UseCases.Series.CreateSeries;
using IoTAnalyzer.UseCases.Series.DeleteSeries;
using IoTAnalyzer.UseCases.Series.GetSeriesById;
using IoTAnalyzer.UseCases.Series.ListSeries;
using IoTAnalyzer.UseCases.Series.UpdateSeries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTAnalyzer.API.Controllers.Series;

[ApiController]
[Route("api/series")]
[TranslateResultToActionResult]
public class SeriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<SeriesDto>>> List(CancellationToken cancellationToken) =>
        await mediator.Send(new ListSeriesQuery(), cancellationToken);

    [HttpGet("{seriesId:int}")]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<SeriesDto>> GetById(int seriesId, CancellationToken cancellationToken) =>
        await mediator.Send(new GetSeriesByIdQuery(seriesId), cancellationToken);

    [HttpPost]
    [Authorize]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<SeriesDto>> Create(SeriesRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSeriesCommand(request.Name!, request.MinValue!.Value, request.MaxValue!.Value,
            request.Color!, request.Icon, request.Unit);
        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Result.Created(result.Value, $"/api/series/{result.Value.Id}") : result;
    }

    [HttpPut("{seriesId:int}")]
    [Authorize]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid, ResultStatus.Conflict)]
    public async Task<Result<SeriesDto>> Update(int seriesId, SeriesRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSeriesCommand(seriesId, request.Name!, request.MinValue!.Value, request.MaxValue!.Value,
            request.Color!, request.Icon, request.Unit);
        return await mediator.Send(command, cancellationToken);
    }

    [HttpDelete("{seriesId:int}")]
    [Authorize]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result> Delete(int seriesId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteSeriesCommand(seriesId), cancellationToken);
        return result.IsSuccess ? Result.NoContent() : result;
    }
}
