using Emerge.Core.Environment;
using Emerge.Core.Genetics;
using Emerge.Core.Organisms;

namespace Emerge.Core.Simulation;

public sealed class Simulation
{
    private readonly Random _random;
    private readonly SimulationConfig _config;

    public World World { get; }
    public int TickCount { get; private set; }
    public int TotalBirths { get; private set; }
    public bool IsRunning { get; private set; } = true;

    public void Start() => IsRunning = true;

    public void Stop() => IsRunning = false;

    public Simulation(SimulationConfig config)
    {
        _config = config;
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
        lock (World.SyncRoot)
        {
            TickCount++;

            SpawnFood();

            var newborns = new List<Organism>();

            foreach (var organism in World.Organisms)
            {
                UpdateOrganism(organism);

                var child = TryReproduce(organism);
                if (child is not null)
                {
                    newborns.Add(child);
                }
            }

            World.Organisms.AddRange(newborns);
            World.Organisms.RemoveAll(o => !o.IsAlive);

            if (World.Organisms.Count == 0)
            {
                IsRunning = false;
            }
        }
    }

    private void SpawnFood()
    {
        int count = (int)_config.FoodSpawnRate;
        for (int i = 0; i < count; i++)
        {
            World.Food.Add(new Food
            {
                X = _random.NextDouble() * World.Width,
                Y = _random.NextDouble() * World.Height
            });
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

        TryEat(organism);
    }

    private void TryEat(Organism organism)
    {
        var eaten = World.Food.FirstOrDefault(f =>
            Distance(organism.X, organism.Y, f.X, f.Y) <= _config.EatDistance);

        if (eaten is not null)
        {
            organism.Energy += eaten.EnergyValue;
            World.Food.Remove(eaten);
        }
    }

    private Organism? TryReproduce(Organism parent)
    {
        if (parent.Energy < _config.ReproductionEnergyThreshold)
        {
            return null;
        }

        parent.Energy -= _config.ReproductionEnergyCost;

        var childGenome = parent.Genome.Mutate(_random, _config.MutationRate, _config.MutationAmount);

        TotalBirths++;

        return new Organism
        {
            Genome = childGenome,
            X = parent.X,
            Y = parent.Y,
            Energy = _config.ReproductionEnergyCost * 0.5,
            Health = 100
        };
    }

    private static double Distance(double x1, double y1, double x2, double y2)
    {
        double dx = x1 - x2;
        double dy = y1 - y2;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}