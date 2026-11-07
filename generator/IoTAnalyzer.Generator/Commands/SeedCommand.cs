using IoTAnalyzer.Generator.Api;
using IoTAnalyzer.Generator.Options;
using IoTAnalyzer.Generator.Values;

namespace IoTAnalyzer.Generator.Commands;

public static class SeedCommand
{
    private record SampleSeries(SeriesRequest Series, string Pattern, double Min, double Max);

    private static readonly SampleSeries[] Samples =
    [
        new(new SeriesRequest("Temperatura - salon", 10, 35, "#d62728", "circle", "°C"), "sine", 18, 26),
        new(new SeriesRequest("Wilgotność - salon", 0, 100, "#1f77b4", "square", "%"), "walk", 35, 65),
        new(new SeriesRequest("Ciśnienie atmosferyczne", 950, 1050, "#2ca02c", "triangle", "hPa"), "walk", 995, 1025),
    ];

    public static async Task<int> RunAsync(CommandLine cli, CancellationToken cancellationToken)
    {
        var apiUrl = cli.Require("api-url", "IOT_API_URL");
        var username = cli.Require("username", "IOT_ADMIN_USERNAME");
        var password = cli.Require("password", "IOT_ADMIN_PASSWORD");
        var count = cli.GetInt("count", 48);
        var interval = cli.GetInterval("interval", TimeSpan.FromMinutes(30));

        using var client = new IoTApiClient(apiUrl);

        var login = await client.LoginAsync(username, password, cancellationToken);
        if (!login.IsSuccess)
        {
            Console.Error.WriteLine($"Logowanie nie powiodło się: {login.Error}");
            return 1;
        }

        var exitCode = 0;
        foreach (var sample in Samples)
        {
            Console.WriteLine();
            Console.WriteLine($"Seria „{sample.Series.Name}”");

            var series = await client.CreateSeriesAsync(login.Value!.AccessToken, sample.Series, cancellationToken);
            if (!series.IsSuccess)
            {
                Console.Error.WriteLine($"  Nie udało się utworzyć serii: {series.Error}");
                return 1;
            }

            var sensor = await client.RegisterSensorAsync(login.Value.AccessToken, $"Czujnik - {sample.Series.Name}", series.Value!.Id, cancellationToken);
            if (!sensor.IsSuccess)
            {
                Console.Error.WriteLine($"  Nie udało się zarejestrować czujnika: {sensor.Error}");
                return 1;
            }

            Console.WriteLine($"  Seria id {series.Value.Id}, czujnik id {sensor.Value!.Id}, klucz API: {sensor.Value.ApiKey}");

            var generator = ValueGeneratorFactory.Create(sample.Pattern, sample.Min, sample.Max, 24, 0.1, null, Random.Shared);
            var result = await SendCommand.SendHistoryAsync(client, sensor.Value.ApiKey, generator, count, interval, null, cancellationToken);
            exitCode = Math.Max(exitCode, result);
        }

        return exitCode;
    }
}
