namespace RayTutorial.Rendering;

public readonly record struct RenderResolution(int Width, int Height)
{
    public static RenderResolution Default { get; } = new(1280, 720);
}
