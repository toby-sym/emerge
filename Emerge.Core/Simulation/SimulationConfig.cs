namespace Emerge.Core.Simulation;

public sealed class SimulationConfig
{
    public required int Seed { get; init; }
    public required double WorldWidth { get; init; }
    public required double WorldHeight { get; init; }
    public int InitialPopulation { get; init; } = 50;
    public double FoodSpawnRate { get; init; } = 15;
    public double EatDistance { get; init; } = 15;
    public double ReproductionEnergyThreshold { get; init; } = 150;
    public double ReproductionEnergyCost { get; init; } = 80;
    public double MutationRate { get; init; } = 0.1;
    public double MutationAmount { get; init; } = 0.2;
}