namespace Emerge.Core.Environment;

public sealed class Food
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public double EnergyValue { get; init; } = 30;
}