namespace Emerge.Core.Genetics;

public sealed class Genome
{
    public required double Speed { get; init; }
    public required double Size { get; init; }
    public required double Metabolism { get; init; }
    public required double Vision { get; init; }

    public Genome Mutate(Random random, double mutationRate, double mutationAmount)
    {
        return new Genome
        {
            Speed = MutateTrait(Speed, random, mutationRate, mutationAmount),
            Size = MutateTrait(Size, random, mutationRate, mutationAmount),
            Metabolism = MutateTrait(Metabolism, random, mutationRate, mutationAmount),
            Vision = MutateTrait(Vision, random, mutationRate, mutationAmount)
        };
    }

    private static double MutateTrait(double value, Random random, double mutationRate, double mutationAmount)
    {
        if (random.NextDouble() >= mutationRate)
        {
            return value;
        }

        double delta = (random.NextDouble() - 0.5) * 2 * mutationAmount;
        return Math.Max(0.01, value + value * delta); // keep traits positive
    }
}