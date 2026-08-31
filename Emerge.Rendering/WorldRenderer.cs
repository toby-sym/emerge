using System;
using System.Collections.Generic;
using System.Linq;
using Emerge.Core.Environment;
using Emerge.Core.Organisms;

namespace Emerge.Rendering;

public static class WorldRenderer
{
    public static IEnumerable<DrawCommand> Render(World world)
    {
        Food[] foodSnapshot;
        Organism[] organismSnapshot;

        lock (world.SyncRoot)
        {
            foodSnapshot = world.Food.ToArray();
            organismSnapshot = world.Organisms.ToArray();
        }

        foreach (var food in foodSnapshot)
        {
            yield return new DrawCommand
            {
                X = food.X,
                Y = food.Y,
                Radius = 2,
                R = 80, G = 200, B = 80
            };
        }

        foreach (var organism in organismSnapshot)
        {
            // Color encodes speed: faster = more red, slower = more blue
            double speedFraction = Math.Clamp((organism.Genome.Speed - 1) / 3.0, 0, 1);
            byte red = (byte)(speedFraction * 255);
            byte blue = (byte)((1 - speedFraction) * 255);

            yield return new DrawCommand
            {
                X = organism.X,
                Y = organism.Y,
                Radius = 3 + organism.Genome.Size,
                R = red, G = 60, B = blue
            };
        }
    }
}