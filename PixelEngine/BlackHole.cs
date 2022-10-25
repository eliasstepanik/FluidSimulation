using System.Numerics;

namespace PixelEngine;

public class BlackHole : Hole
{
    public BlackHole(Vector2 position, int radius, float mass) : base(position, mass)
    {
        this.radius = radius;
    }

    public override void Spawn(Fluid fluid)
    {
        fluid.reduceDensity((int)position.X, (int)position.Y, mass, radius);
    }

    public override Vector2 position { get; set; }
    public int radius { get; set; }
    public override float mass { get; set; }
}