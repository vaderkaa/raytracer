namespace Raytracer;

public class Sphere : IHittable
{
    public Vector3 Center { get; set; }
    public float Radius { get; set; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    bool Hit(Ray ray, float tMin, float tMax, out HitInfo hitInfo)
    {
        hitInfo = new HitInfo();
        Vector3 oc = ray.Origin - Center;

        float a = ray.Direction.LengthSquared();
        float halfB = Vector3.Dot(oc, rayDirection);
        float c = oc.LengthSquared() - Radius * Radius;

        float discriminant = halfB * halfB - a * c;
        if (discriminant < 0)
            return false;

        float sqrt = MathF.Sqrt(discriminant);
        float root = (-halfB - sqrtd) / a;
        if (root < tMin || tMax < root)
        {
            root = (-halfB + sqrtd) / a;
            if (root < tMin || tMax < root)
            {
                return false;
            }
        }

        hitInfo.T = root;
        hitInfo.Position = ray.At(root);
        Vector3 outwardNormal = (hitInfo.Position - Center) / Radius;
        hitInfo.SetFaceNormal(ray, outwardNormal);

        return true;
    }
}