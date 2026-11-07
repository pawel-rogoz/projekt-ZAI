using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using IoTAnalyzer.UseCases.Measurements;
using IoTAnalyzer.UseCases.Measurements.CreateMeasurement;
using IoTAnalyzer.UseCases.Measurements.GetMeasurementById;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoTAnalyzer.API.Controllers.Measurements;

[ApiController]
[Route("api/measurements")]
[TranslateResultToActionResult]
public class MeasurementsController(IMediator mediator) : ControllerBase
{
    public const string ApiKeyHeader = "X-API-Key";

    [HttpGet]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<List<MeasurementDto>>> List([FromQuery] ListMeasurementsRequest request, CancellationToken cancellationToken) =>
        await mediator.Send(request.ToQuery(), cancellationToken);

    [HttpGet("{measurementId:long}")]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<MeasurementDto>> GetById(long measurementId, CancellationToken cancellationToken) =>
        await mediator.Send(new GetMeasurementByIdQuery(measurementId), cancellationToken);

    [HttpPost]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Unauthorized)]
    public async Task<Result<MeasurementDto>> Create(
        [FromHeader(Name = ApiKeyHeader)] string? apiKey,
        CreateMeasurementRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateMeasurementCommand(apiKey, request.Value!.Value, request.Timestamp?.UtcDateTime);
        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Result.Created(result.Value, $"/api/measurements/{result.Value.Id}") : result;
    }
}
