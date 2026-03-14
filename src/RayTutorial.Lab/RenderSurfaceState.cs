using RayTutorial.Domain;
using RayTutorial.Rendering;

namespace RayTutorial.Lab;

public sealed record RenderSurfaceState(
    string SurfaceId,
    string SceneId,
    RenderResolution Resolution,
    string RenderMode,
    string OutputSetId,
    IReadOnlyList<AovKind> EnabledOutputs,
    int SamplesPerPixel,
    int MaxBounces);
