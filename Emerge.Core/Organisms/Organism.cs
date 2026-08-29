namespace Emerge.Core.Organisms;

public sealed class Organism
{
    public required Genetics.Genome Genome { get; init; }

    public double X { get; set; }
    public double Y { get; set; }
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }

    public double Energy { get; set; }
    public double Health { get; set; }
    public int Age { get; set; }

    public bool IsAlive => Health > 0 && Energy > 0;
}