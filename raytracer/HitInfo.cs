namespace Raytracer;

public struct HitInfo
{
    public Vector3 Position;
    public Vector3 Normal;
    public float T;
    public bool FrontFace;

    public void SetFaceNormal(Ray r, Vector3 outwardNormal)
    {
        if(Vector3.Dot(r.Direction, outwardNormal) < 0)
        {
            FrontFace = true;
            Normal = outwardNormal;
        }
        else
        {
            FrontFace = false;
            Normal = -outwardNormal;
        }
    }
}