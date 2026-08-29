namespace Emerge.Rendering;

public readonly struct DrawCommand
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Radius { get; init; }
    public required byte R { get; init; }
    public required byte G { get; init; }
    public required byte B { get; init; }
}