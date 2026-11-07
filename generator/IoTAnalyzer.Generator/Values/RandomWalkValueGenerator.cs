namespace IoTAnalyzer.Generator.Values;

public class RandomWalkValueGenerator(double min, double max, double step, Random random) : IValueGenerator
{
    private double? _current;

    public double Next(int index)
    {
        _current ??= min + random.NextDouble() * (max - min);
        _current = Math.Clamp(_current.Value + (random.NextDouble() * 2 - 1) * step, min, max);
        return _current.Value;
    }
}
