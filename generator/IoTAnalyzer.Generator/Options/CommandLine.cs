using System.Globalization;

namespace IoTAnalyzer.Generator.Options;

public class CommandLine
{
    private readonly Dictionary<string, string> _values = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _flags = new(StringComparer.OrdinalIgnoreCase);

    public string Command { get; }

    public CommandLine(string[] args)
    {
        var rest = args.AsEnumerable();
        if (args.Length > 0 && !args[0].StartsWith("--"))
        {
            Command = args[0].ToLowerInvariant();
            rest = args.Skip(1);
        }
        else
        {
            Command = "send";
        }

        var list = rest.ToList();
        for (var i = 0; i < list.Count; i++)
        {
            if (!list[i].StartsWith("--"))
                throw new ArgumentException($"Nieoczekiwany argument: {list[i]}");

            var name = list[i][2..];
            var eq = name.IndexOf('=');
            if (eq >= 0)
                _values[name[..eq]] = name[(eq + 1)..];
            else if (i + 1 < list.Count && !list[i + 1].StartsWith("--"))
                _values[name] = list[++i];
            else
                _flags.Add(name);
        }
    }

    public bool Has(string name) => _flags.Contains(name) || _values.ContainsKey(name);

    public string? Get(string name, string? environmentVariable = null)
    {
        if (_values.TryGetValue(name, out var value))
            return value;
        return environmentVariable is null ? null : Environment.GetEnvironmentVariable(environmentVariable);
    }

    public string Require(string name, string? environmentVariable = null) =>
        Get(name, environmentVariable)
        ?? throw new ArgumentException(environmentVariable is null
            ? $"Brak wymaganego parametru --{name}."
            : $"Brak wymaganego parametru --{name} (lub zmiennej {environmentVariable}).");

    public int GetInt(string name, int defaultValue)
    {
        var text = Get(name);
        if (text is null) return defaultValue;
        if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) || value < 0)
            throw new ArgumentException($"Parametr --{name} musi być nieujemną liczbą całkowitą.");
        return value;
    }

    public double GetDouble(string name, double defaultValue)
    {
        var text = Get(name);
        if (text is null) return defaultValue;
        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) || !double.IsFinite(value))
            throw new ArgumentException($"Parametr --{name} musi być liczbą (separator dziesiętny: kropka).");
        return value;
    }

    public TimeSpan GetInterval(string name, TimeSpan defaultValue)
    {
        var text = Get(name);
        if (text is null) return defaultValue;
        if (!IntervalParser.TryParse(text, out var interval))
            throw new ArgumentException($"Parametr --{name} ma niepoprawny format (np. 30s, 5m, 1h).");
        return interval;
    }

    public DateTimeOffset? GetDate(string name)
    {
        var text = Get(name);
        if (text is null) return null;
        if (!DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date))
            throw new ArgumentException($"Parametr --{name} musi być datą ISO 8601, np. 2026-11-01T00:00:00Z.");
        return date;
    }
}
