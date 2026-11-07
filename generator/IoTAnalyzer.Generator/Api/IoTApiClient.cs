using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IoTAnalyzer.Generator.Api;

public class IoTApiClient : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly HttpClient _http;

    public IoTApiClient(string apiUrl)
    {
        if (!Uri.TryCreate(apiUrl.TrimEnd('/') + "/", UriKind.Absolute, out var baseAddress))
            throw new ArgumentException($"Niepoprawny adres API: {apiUrl}");

        _http = new HttpClient { BaseAddress = baseAddress, Timeout = TimeSpan.FromSeconds(30) };
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<ApiResult<MeasurementResponse>> SendMeasurementAsync(string apiKey, double value, DateTimeOffset? timestamp, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/measurements")
        {
            Content = JsonContent.Create(new MeasurementRequest(value, timestamp?.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture)), options: JsonOptions),
        };
        request.Headers.Add("X-API-Key", apiKey);
        return await SendAsync<MeasurementResponse>(request, cancellationToken);
    }

    public async Task<ApiResult<LoginResponse>> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
        {
            Content = JsonContent.Create(new { username, password }, options: JsonOptions),
        };
        return await SendAsync<LoginResponse>(request, cancellationToken);
    }

    public async Task<ApiResult<SeriesResponse>> CreateSeriesAsync(string token, SeriesRequest series, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/series")
        {
            Content = JsonContent.Create(series, options: JsonOptions),
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await SendAsync<SeriesResponse>(request, cancellationToken);
    }

    public async Task<ApiResult<SensorResponse>> RegisterSensorAsync(string token, string name, int seriesId, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/sensors")
        {
            Content = JsonContent.Create(new { name, seriesId }, options: JsonOptions),
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await SendAsync<SensorResponse>(request, cancellationToken);
    }

    private async Task<ApiResult<T>> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await _http.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<T>.Failure(null, $"Brak połączenia z API: {ex.Message}");
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return ApiResult<T>.Failure(null, "Przekroczono czas oczekiwania na odpowiedź API.");
        }

        using (response)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            if (response.IsSuccessStatusCode)
                return ApiResult<T>.Success(response.StatusCode, JsonSerializer.Deserialize<T>(body, JsonOptions)!);

            return ApiResult<T>.Failure(response.StatusCode, DescribeError(response.StatusCode, body));
        }
    }

    private static string DescribeError(HttpStatusCode status, string body)
    {
        string? detail = null;
        try
        {
            var problem = JsonSerializer.Deserialize<ProblemResponse>(body, JsonOptions);
            detail = problem?.Detail ?? problem?.Title;
        }
        catch (JsonException)
        {
        }

        var reason = status switch
        {
            HttpStatusCode.Unauthorized => "Odmowa dostępu (sprawdź klucz API lub dane logowania)",
            HttpStatusCode.UnprocessableEntity => "Wynik odrzucony przez serwer",
            HttpStatusCode.BadRequest => "Niepoprawne żądanie",
            HttpStatusCode.TooManyRequests => "Zbyt wiele prób, spróbuj za chwilę",
            _ => $"Błąd serwera",
        };

        return detail is null ? $"{reason} [HTTP {(int)status}]" : $"{reason} [HTTP {(int)status}]: {detail}";
    }

    public void Dispose() => _http.Dispose();
}
