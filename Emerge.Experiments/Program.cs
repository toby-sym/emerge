using Emerge.Core.Simulation;
using System.Linq;

var config = new SimulationConfig
{
    Seed = 48192837,
    WorldWidth = 800,
    WorldHeight = 600,
    InitialPopulation = 50
};

var simulation = new Simulation(config);

for (int i = 0; i < 500 && simulation.IsRunning; i++)
{
    simulation.Tick();

    if (simulation.TickCount % 50 == 0)
    {
        var pop = simulation.World.Organisms;
        double avgSpeed = pop.Count > 0 ? pop.Average(o => o.Genome.Speed) : 0;
        Console.WriteLine($"Tick {simulation.TickCount}: Population={pop.Count}, Food={simulation.World.Food.Count}, Births={simulation.TotalBirths}, AvgSpeed={avgSpeed:F2}");
    }
}

Console.WriteLine($"Final — Ticks: {simulation.TickCount}, Population: {simulation.World.Organisms.Count}, Food remaining: {simulation.World.Food.Count}");