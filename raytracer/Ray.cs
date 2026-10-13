namespace Raytracer;

public struct Ray
{
    public readonly Vector3 Origin { get; }
    public readonly Vector3 Direction { get; }

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vector3 At(float t) => Origin + t * Direction;
}
