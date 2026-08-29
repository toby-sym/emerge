using Emerge.Core.Simulation;

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
        Console.WriteLine($"Tick {simulation.TickCount}: Population={simulation.World.Organisms.Count}, Food={simulation.World.Food.Count}");
    }
}

Console.WriteLine($"Final — Ticks: {simulation.TickCount}, Population: {simulation.World.Organisms.Count}, Food remaining: {simulation.World.Food.Count}");