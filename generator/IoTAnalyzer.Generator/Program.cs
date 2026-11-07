using System.Text;
using IoTAnalyzer.Generator;
using IoTAnalyzer.Generator.Commands;
using IoTAnalyzer.Generator.Options;

Console.OutputEncoding = Encoding.UTF8;

using var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cancellation.Cancel();
};

try
{
    var cli = new CommandLine(args);
    if (cli.Has("help") || cli.Command is "help")
    {
        Console.WriteLine(HelpText.Value);
        return 0;
    }

    return cli.Command switch
    {
        "send" => await SendCommand.RunAsync(cli, cancellation.Token),
        "seed" => await SeedCommand.RunAsync(cli, cancellation.Token),
        _ => throw new ArgumentException($"Nieznane polecenie: {cli.Command}. Dostępne: send, seed, help."),
    };
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Błąd: {ex.Message}");
    Console.Error.WriteLine("Użyj --help, aby zobaczyć listę parametrów.");
    return 2;
}
catch (OperationCanceledException)
{
    return 130;
}
