using RayTutorial.Domain;

namespace RayTutorial.Rendering;

public sealed record RenderSurfaceDescriptor(
    string SurfaceId,
    string SceneId,
    RenderResolution Resolution,
    RenderMode RenderMode,
    RenderQualitySettings Quality,
    IReadOnlyList<AovKind> EnabledOutputs);
