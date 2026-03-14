using RayTutorial.Domain;

namespace RayTutorial.Rendering;

public sealed record RenderSurfaceDescriptor(
    string SurfaceId,
    string SceneId,
    RenderResolution Resolution,
    string RenderMode,
    int SamplesPerPixel,
    int MaxBounces,
    IReadOnlyList<AovKind> EnabledOutputs);
