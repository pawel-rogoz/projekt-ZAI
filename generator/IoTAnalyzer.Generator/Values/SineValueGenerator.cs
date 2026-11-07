namespace IoTAnalyzer.Generator.Values;

public class SineValueGenerator(double min, double max, int period, double noise, Random random) : IValueGenerator
{
    public double Next(int index)
    {
        var middle = (min + max) / 2;
        var amplitude = (max - min) / 2 * (1 - noise);
        var value = middle + amplitude * Math.Sin(2 * Math.PI * index / Math.Max(period, 1));
        var jitter = (random.NextDouble() * 2 - 1) * noise * (max - min) / 2;
        return Math.Clamp(value + jitter, min, max);
    }
}
