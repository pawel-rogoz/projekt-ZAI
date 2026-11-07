using System.Net;

namespace IoTAnalyzer.Generator.Api;

public record MeasurementRequest(double Value, string? Timestamp);

public record MeasurementResponse(long Id, int SeriesId, double Value, DateTimeOffset Timestamp);

public record LoginResponse(string AccessToken, string TokenType, int ExpiresIn);

public record SeriesRequest(string Name, double MinValue, double MaxValue, string Color, string? Icon, string? Unit);

public record SeriesResponse(int Id, string Name, double MinValue, double MaxValue);

public record SensorResponse(int Id, string Name, int SeriesId, string ApiKey);

public record ProblemResponse(string? Title, int? Status, string? Detail);

public record ApiResult<T>(bool IsSuccess, HttpStatusCode? StatusCode, T? Value, string? Error)
{
    public static ApiResult<T> Success(HttpStatusCode status, T value) => new(true, status, value, null);
    public static ApiResult<T> Failure(HttpStatusCode? status, string error) => new(false, status, default, error);
}
