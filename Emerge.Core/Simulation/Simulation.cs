using Emerge.Core.Environment;
using Emerge.Core.Genetics;
using Emerge.Core.Organisms;

namespace Emerge.Core.Simulation;

public sealed class Simulation
{
    private readonly Random _random;

    public World World { get; }
    public int TickCount { get; private set; }
    public bool IsRunning { get; private set; } = true;

    public Simulation(SimulationConfig config)
    {
        _random = new Random(config.Seed);
        World = new World { Width = config.WorldWidth, Height = config.WorldHeight };

        for (int i = 0; i < config.InitialPopulation; i++)
        {
            World.Organisms.Add(CreateRandomOrganism());
        }
    }

    private Organism CreateRandomOrganism()
    {
        var genome = new Genome
        {
            Speed = _random.NextDouble() * 2 + 1,
            Size = _random.NextDouble() * 2 + 1,
            Metabolism = _random.NextDouble() * 0.5 + 0.5,
            Vision = _random.NextDouble() * 50 + 20
        };

        return new Organism
        {
            Genome = genome,
            X = _random.NextDouble() * World.Width,
            Y = _random.NextDouble() * World.Height,
            Energy = 100,
            Health = 100
        };
    }

    public void Tick()
    {
        TickCount++;

        foreach (var organism in World.Organisms)
        {
            UpdateOrganism(organism);
        }

        World.Organisms.RemoveAll(o => !o.IsAlive);

        if (World.Organisms.Count == 0)
        {
            IsRunning = false;
        }
    }

    private void UpdateOrganism(Organism organism)
    {
        organism.VelocityX = (_random.NextDouble() - 0.5) * organism.Genome.Speed;
        organism.VelocityY = (_random.NextDouble() - 0.5) * organism.Genome.Speed;

        organism.X = Math.Clamp(organism.X + organism.VelocityX, 0, World.Width);
        organism.Y = Math.Clamp(organism.Y + organism.VelocityY, 0, World.Height);

        organism.Age++;
        organism.Energy -= organism.Genome.Metabolism;
    }
}