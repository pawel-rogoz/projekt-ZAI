using System.Text.RegularExpressions;
using Ardalis.Result;

namespace IoTAnalyzer.UseCases.Series;

public static partial class SeriesValidator
{
    public const int NameMaxLength = 100;
    public const int IconMaxLength = 50;
    public const int UnitMaxLength = 20;

    [GeneratedRegex("^#[0-9A-Fa-f]{6}$")]
    private static partial Regex ColorRegex();

    public static List<ValidationError> Validate(string name, double minValue, double maxValue, string color, string? icon, string? unit)
    {
        var errors = new List<ValidationError>();

        var trimmedName = name?.Trim() ?? string.Empty;
        if (trimmedName.Length is 0 or > NameMaxLength)
            errors.Add(Error("name", $"Nazwa musi mieć od 1 do {NameMaxLength} znaków."));
        if (!double.IsFinite(minValue) || !double.IsFinite(maxValue))
            errors.Add(Error("minValue", "Granice przedziału muszą być skończonymi liczbami."));
        else if (minValue >= maxValue)
            errors.Add(Error("maxValue", "Wartość maxValue musi być większa niż minValue."));
        if (color is null || !ColorRegex().IsMatch(color))
            errors.Add(Error("color", "Kolor musi mieć format #RRGGBB."));
        if (icon?.Trim().Length > IconMaxLength)
            errors.Add(Error("icon", $"Nazwa ikony może mieć najwyżej {IconMaxLength} znaków."));
        if (unit?.Trim().Length > UnitMaxLength)
            errors.Add(Error("unit", $"Jednostka może mieć najwyżej {UnitMaxLength} znaków."));

        return errors;
    }

    private static ValidationError Error(string field, string message) =>
        new() { Identifier = field, ErrorMessage = message };
}
