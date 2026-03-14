using RayTutorial.Domain;
using RayTutorial.Rendering;

namespace RayTutorial.Lab;

public sealed record RenderSurfaceState(
    string SurfaceId,
    string SceneId,
    RenderResolution Resolution,
    RenderMode RenderMode,
    IReadOnlyList<AovKind> EnabledOutputs,
    int SamplesPerPixel,
    int MaxBounces);
