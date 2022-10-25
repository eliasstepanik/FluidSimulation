using System.Numerics;

namespace PixelEngine;

public abstract class Hole
{
    public Hole(Vector2 position, float mass)
    {
        this.position = position;
        this.mass = mass;
    }
    public abstract void Spawn(Fluid fluid);
    public abstract Vector2 position { get; set; }
    public abstract float mass { get; set; }
}