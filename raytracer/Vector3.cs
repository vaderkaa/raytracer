namespace Raytracer;

public struct Vector3
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString() => $"{X}, {Y}, {Z}";

    public static Vector3 operator -(Vector3 v) => new Vector3(-v.X, -v.Y, -v.Z);
    public static Vector3 operator +(Vector3 v, Vector3 u) => new Vector3(v.X + u.X, v.Y + u.Y, v.Z + u.Z);
    public static Vector3 operator -(Vector3 v, Vector3 u) => new Vector3(v.X - u.X, v.Y - u.Y, v.Z - u.Z);
    public static Vector3 operator *(Vector3 v, float k) => new Vector3(k * v.X, k * v.Y, k * v.Z);
    public static Vector3 operator *(float k, Vector3 v) => v * k;
    public static Vector3 operator /(Vector3 v, float k) => (1.0f / k) * v;
    public static Vector3 operator *(Vector3 v, Vector3 u) => new Vector3(v.X * u.X, v.Y * u.Y, v.Z * u.Z);
    public float LengthSquared() => X * X + Y * Y + Z * Z;
    public float Length() => MathF.Sqrt(LengthSquared());
    public static float Dot(Vector3 v, Vector3 u) => v.X * u.X + v.Y * u.Y + v.Z * u.Z;
    public static Vector3 Cross(Vector3 u, Vector3 v) => new Vector3(   u.Y * v.Z - u.Z * v.Y,
                                                                        u.Z * v.X - u.X * v.Z,
                                                                        u.X * v.Y - u.Y * v.X   );
    public Vector3 Normalize() => this / Length();
}
