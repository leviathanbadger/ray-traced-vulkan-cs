namespace RayTutorial.Rendering;

public sealed record RenderSurfaceDescriptor(
    string SurfaceId,
    string SceneId,
    RenderResolution Resolution);
