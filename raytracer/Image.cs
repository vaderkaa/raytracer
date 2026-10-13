using System.Text;

namespace Raytracer;

public class Image
{
    private Vector3[,] _pixels;

    public int Width => _pixels.GetLength(0);
    public int Height => _pixels.GetLength(1);

    public Image(int width, int height)
    {
        _pixels = new Vector3[width, height];
    }
    
    public Vector3 this[int x, int y]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = value;
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"P3");
        result.AppendLine($"{Width} {Height}");
        result.AppendLine($"255");

        for(int j = 0; j < Height; j++)
        {
            for(int i = 0; i < Width; i++)
            {
                var color = this[i, j];
                int r = Math.Clamp((int)(color.X * 255.99), 0, 255);
                int g = Math.Clamp((int)(color.Y * 255.99), 0, 255);
                int b = Math.Clamp((int)(color.Z * 255.99), 0, 255);
                result.AppendLine($"{r} {g} {b}");
            }
        }

        return result.ToString();
    }
}
