using System.Globalization;
using System.Net;
using IoTAnalyzer.Generator.Api;
using IoTAnalyzer.Generator.Options;
using IoTAnalyzer.Generator.Values;

namespace IoTAnalyzer.Generator.Commands;

public static class SendCommand
{
    public static async Task<int> RunAsync(CommandLine cli, CancellationToken cancellationToken)
    {
        var apiUrl = cli.Require("api-url", "IOT_API_URL");
        var apiKey = cli.Require("api-key", "IOT_API_KEY");
        var mode = (cli.Get("mode") ?? "history").ToLowerInvariant();
        var count = cli.GetInt("count", mode == "history" ? 30 : 0);
        var interval = cli.GetInterval("interval", mode == "history" ? TimeSpan.FromMinutes(30) : TimeSpan.FromSeconds(10));
        var generator = CreateGenerator(cli);

        using var client = new IoTApiClient(apiUrl);
        return mode switch
        {
            "history" => await SendHistoryAsync(client, apiKey, generator, count, interval, cli.GetDate("start"), cancellationToken),
            "live" => await SendLiveAsync(client, apiKey, generator, count, interval, cancellationToken),
            _ => throw new ArgumentException("Parametr --mode przyjmuje wartości: history, live."),
        };
    }

    public static IValueGenerator CreateGenerator(CommandLine cli) =>
        ValueGeneratorFactory.Create(
            cli.Get("pattern") ?? "sine",
            cli.GetDouble("min", 0),
            cli.GetDouble("max", 100),
            cli.GetInt("period", 24),
            cli.GetDouble("noise", 0.1),
            cli.Has("step") ? cli.GetDouble("step", 1) : null,
            cli.Has("seed") ? new Random(cli.GetInt("seed", 0)) : Random.Shared);

    public static async Task<int> SendHistoryAsync(IoTApiClient client, string apiKey, IValueGenerator generator,
        int count, TimeSpan interval, DateTimeOffset? start, CancellationToken cancellationToken)
    {
        if (count == 0)
            throw new ArgumentException("W trybie history parametr --count musi być większy od zera.");

        var first = start ?? DateTimeOffset.UtcNow - interval * (count - 1);
        Console.WriteLine($"Wysyłanie {count} wyników z przeszłości od {first:u} co {interval}.");

        var stats = new SendStats();
        for (var i = 0; i < count && !cancellationToken.IsCancellationRequested; i++)
        {
            var result = await SendOneAsync(client, apiKey, generator.Next(i), first + interval * i, cancellationToken);
            if (!stats.Register(result))
                return stats.Summarize();
        }
        return stats.Summarize();
    }

    private static async Task<int> SendLiveAsync(IoTApiClient client, string apiKey, IValueGenerator generator,
        int count, TimeSpan interval, CancellationToken cancellationToken)
    {
        Console.WriteLine(count > 0
            ? $"Wysyłanie {count} bieżących wyników co {interval}. Ctrl+C przerywa."
            : $"Wysyłanie bieżących wyników co {interval} aż do przerwania (Ctrl+C).");

        var stats = new SendStats();
        using var timer = new PeriodicTimer(interval);
        var index = 0;
        do
        {
            var result = await SendOneAsync(client, apiKey, generator.Next(index), DateTimeOffset.UtcNow, cancellationToken);
            if (!stats.Register(result))
                break;
            index++;
        }
        while ((count == 0 || index < count) && await WaitAsync(timer, cancellationToken));

        return stats.Summarize();
    }

    private static async Task<bool> WaitAsync(PeriodicTimer timer, CancellationToken cancellationToken)
    {
        try
        {
            return await timer.WaitForNextTickAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    private static async Task<ApiResult<MeasurementResponse>> SendOneAsync(IoTApiClient client, string apiKey,
        double value, DateTimeOffset timestamp, CancellationToken cancellationToken)
    {
        var rounded = Math.Round(value, 2);
        var result = await client.SendMeasurementAsync(apiKey, rounded, timestamp, cancellationToken);
        var text = rounded.ToString(CultureInfo.InvariantCulture);

        if (result.IsSuccess)
            Console.WriteLine($"  ✓ {timestamp:u}  {text,10}  → zapisano (id {result.Value!.Id}, seria {result.Value.SeriesId})");
        else
            Console.Error.WriteLine($"  ✗ {timestamp:u}  {text,10}  → {result.Error}");

        return result;
    }

    private sealed class SendStats
    {
        private int _sent;
        private int _rejected;

        public bool Register(ApiResult<MeasurementResponse> result)
        {
            if (result.IsSuccess)
            {
                _sent++;
                return true;
            }

            _rejected++;
            if (result.StatusCode is HttpStatusCode.Unauthorized or null)
            {
                Console.Error.WriteLine("Przerwano wysyłanie.");
                return false;
            }
            return true;
        }

        public int Summarize()
        {
            Console.WriteLine($"Zapisano: {_sent}, odrzucono: {_rejected}.");
            return _rejected == 0 ? 0 : 1;
        }
    }
}
