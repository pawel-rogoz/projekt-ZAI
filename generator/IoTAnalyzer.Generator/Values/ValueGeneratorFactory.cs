namespace IoTAnalyzer.Generator.Values;

public static class ValueGeneratorFactory
{
    public static IValueGenerator Create(string pattern, double min, double max, int period, double noise, double? step, Random random)
    {
        if (min >= max)
            throw new ArgumentException("Parametr --min musi być mniejszy od --max.");

        return pattern.ToLowerInvariant() switch
        {
            "random" => new RandomValueGenerator(min, max, random),
            "sine" => new SineValueGenerator(min, max, period, noise, random),
            "walk" => new RandomWalkValueGenerator(min, max, step ?? (max - min) / 20, random),
            _ => throw new ArgumentException("Parametr --pattern przyjmuje wartości: random, sine, walk."),
        };
    }
}
