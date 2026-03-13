namespace RayTutorial.Rendering;

public readonly record struct ViewportSize(int Width, int Height)
{
    public static ViewportSize Empty { get; } = new(0, 0);

    public bool IsEmpty => Width <= 0 || Height <= 0;
}
