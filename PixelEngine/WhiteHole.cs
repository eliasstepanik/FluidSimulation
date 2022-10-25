using System.Numerics;

namespace PixelEngine;

public class WhiteHole : Hole 
{
    public override void Spawn(Fluid fluid)
    {
        fluid.addDensity((int) position.X, (int) position.Y,  mass);
        fluid.addVelocity((int) position.X, (int) position.Y,  direction.X * speed, direction.Y * speed);
    }

    public override Vector2 position { get; set; }
    public Vector2 direction { get; set; }
    public override float mass { get; set; }
    public float speed { get; set; }

    public WhiteHole(Vector2 position, Vector2 direction, float mass, float speed) : base(position, mass)
    {
        this.direction = direction;
        this.speed = speed;
    }
}