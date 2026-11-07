using System.Globalization;

namespace IoTAnalyzer.Generator.Options;

public static class IntervalParser
{
    public static bool TryParse(string text, out TimeSpan interval)
    {
        interval = default;
        text = text.Trim().ToLowerInvariant();
        if (text.Length == 0) return false;

        var unit = char.IsLetter(text[^1]) ? text[^1] : 's';
        var number = char.IsLetter(text[^1]) ? text[..^1] : text;
        if (!double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) || value <= 0)
            return false;

        interval = unit switch
        {
            's' => TimeSpan.FromSeconds(value),
            'm' => TimeSpan.FromMinutes(value),
            'h' => TimeSpan.FromHours(value),
            'd' => TimeSpan.FromDays(value),
            _ => TimeSpan.Zero,
        };
        return interval > TimeSpan.Zero;
    }
}
