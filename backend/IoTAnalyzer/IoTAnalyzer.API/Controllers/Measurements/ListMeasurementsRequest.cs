using System.ComponentModel.DataAnnotations;
using IoTAnalyzer.UseCases.Measurements.ListMeasurements;

namespace IoTAnalyzer.API.Controllers.Measurements;

public class ListMeasurementsRequest
{
    [RegularExpression(@"^\s*\d+\s*(,\s*\d+\s*)*$", ErrorMessage = "Parametr series musi być listą identyfikatorów rozdzielonych przecinkami.")]
    public string? Series { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    [RegularExpression("^-?timestamp$", ErrorMessage = "Dozwolone wartości: timestamp, -timestamp.")]
    public string? Sort { get; set; }

    [Range(1, ListMeasurementsQuery.MaxLimit)]
    public int? Limit { get; set; }

    public ListMeasurementsQuery ToQuery()
    {
        var seriesIds = string.IsNullOrWhiteSpace(Series)
            ? []
            : Series.Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToList();

        return new ListMeasurementsQuery(
            seriesIds,
            From?.UtcDateTime,
            To?.UtcDateTime,
            Sort == "-timestamp",
            Limit ?? ListMeasurementsQuery.DefaultLimit);
    }
}
