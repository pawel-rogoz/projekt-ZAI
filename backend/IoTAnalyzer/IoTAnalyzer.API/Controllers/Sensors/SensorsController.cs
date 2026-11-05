using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using IoTAnalyzer.UseCases.Sensors;
using IoTAnalyzer.UseCases.Sensors.GetSensorById;
using IoTAnalyzer.UseCases.Sensors.ListSensors;
using IoTAnalyzer.UseCases.Sensors.RegisterSensor;
using IoTAnalyzer.UseCases.Sensors.UnregisterSensor;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoTAnalyzer.API.Controllers.Sensors;

[ApiController]
[Route("api/sensors")]
[TranslateResultToActionResult]
public class SensorsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<SensorDto>>> List(CancellationToken cancellationToken) =>
        await mediator.Send(new ListSensorsQuery(), cancellationToken);

    [HttpGet("{sensorId:int}")]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<SensorDto>> GetById(int sensorId, CancellationToken cancellationToken) =>
        await mediator.Send(new GetSensorByIdQuery(sensorId), cancellationToken);

    [HttpPost]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<RegisteredSensorDto>> Register(RegisterSensorRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegisterSensorCommand(request.Name!, request.SeriesId!.Value), cancellationToken);
        if (!result.IsSuccess)
            return result;

        Response.Headers.CacheControl = "no-store";
        return Result.Created(result.Value, $"/api/sensors/{result.Value.Id}");
    }

    [HttpDelete("{sensorId:int}")]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result> Unregister(int sensorId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UnregisterSensorCommand(sensorId), cancellationToken);
        return result.IsSuccess ? Result.NoContent() : result;
    }
}
