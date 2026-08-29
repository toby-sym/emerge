namespace Emerge.Core.Environment;

public sealed class World
{
    public required double Width { get; init; }
    public required double Height { get; init; }
    public List<Organisms.Organism> Organisms { get; } = new();
}