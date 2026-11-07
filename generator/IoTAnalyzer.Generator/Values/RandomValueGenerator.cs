namespace IoTAnalyzer.Generator.Values;

public class RandomValueGenerator(double min, double max, Random random) : IValueGenerator
{
    public double Next(int index) => min + random.NextDouble() * (max - min);
}
