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
}

Console.WriteLine($"Ticks run: {simulation.TickCount}, Population: {simulation.World.Organisms.Count}, Food remaining: {simulation.World.Food.Count}");