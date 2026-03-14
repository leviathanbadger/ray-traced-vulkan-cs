using RayTutorial.Rendering;

namespace RayTutorial.Lab;

public sealed record RenderSurfaceState(
    string SurfaceId,
    string SceneId,
    RenderResolution Resolution,
    string CameraId,
    string RenderMode,
    string OutputSetId,
    int SamplesPerPixel,
    int MaxBounces);
