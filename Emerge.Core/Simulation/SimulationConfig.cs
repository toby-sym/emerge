namespace Emerge.Core.Simulation;

public sealed class SimulationConfig
{
    public required int Seed { get; init; }
    public required double WorldWidth { get; init; }
    public required double WorldHeight { get; init; }
    public int InitialPopulation { get; init; } = 50;
    public double FoodSpawnRate { get; init; } = 15;
    public double EatDistance { get; init; } = 15;
}