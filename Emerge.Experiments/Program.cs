using Emerge.Core.Simulation;

int seed = 48192837;
int ticks = 500;
int logInterval = 50;

for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--seed" && i + 1 < args.Length && int.TryParse(args[i + 1], out var s))
    {
        seed = s;
    }
    else if (args[i] == "--ticks" && i + 1 < args.Length && int.TryParse(args[i + 1], out var t))
    {
        ticks = t;
    }
}

RunSimulation(seed, ticks, logInterval);

static void RunSimulation(int seed, int maxTicks, int logInterval)
{
    var config = new SimulationConfig
    {
        Seed = seed,
        WorldWidth = 800,
        WorldHeight = 600,
        InitialPopulation = 50
    };

    var simulation = new Simulation(config);

    Console.WriteLine($"=== Seed: {seed}, MaxTicks: {maxTicks} ===");

    for (int i = 0; i < maxTicks && simulation.IsRunning; i++)
    {
        simulation.Tick();

        if (simulation.TickCount % logInterval == 0)
        {
            var pop = simulation.World.Organisms;
            double avgSpeed = pop.Count > 0 ? pop.Average(o => o.Genome.Speed) : 0;
            Console.WriteLine($"Tick {simulation.TickCount}: Population={pop.Count}, Food={simulation.World.Food.Count}, Births={simulation.TotalBirths}, AvgSpeed={avgSpeed:F2}");
        }
    }

    var finalPop = simulation.World.Organisms;
    Console.WriteLine($"Final — Ticks: {simulation.TickCount}, Population: {finalPop.Count}, Extinct: {!simulation.IsRunning}");
}