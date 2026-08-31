namespace Emerge.Core.Environment;

public sealed class World
{
    public required double Width { get; init; }
    public required double Height { get; init; }
    public List<Organisms.Organism> Organisms { get; } = new();
    public List<Food> Food { get; } = new();
    public object SyncRoot { get; } = new();
}