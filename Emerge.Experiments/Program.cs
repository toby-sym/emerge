using Emerge.Core.Simulation;

const int ticksPerRun = 500;
const int logInterval = 50;

int[] seeds = { 48192837, 12345, 999, 2026, 555111 };

foreach (var seed in seeds)
{
    RunSimulation(seed, ticksPerRun, logInterval);
    Console.WriteLine();
}

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

    Console.WriteLine($"=== Seed: {seed} ===");

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