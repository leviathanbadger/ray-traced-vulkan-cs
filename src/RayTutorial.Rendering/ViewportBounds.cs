namespace RayTutorial.Rendering;

public readonly record struct ViewportBounds(int X, int Y, int Width, int Height)
{
    public static ViewportBounds Empty { get; } = new(0, 0, 0, 0);
}
