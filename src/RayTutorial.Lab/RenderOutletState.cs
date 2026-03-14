using RayTutorial.Domain;
using RayTutorial.Rendering;

namespace RayTutorial.Lab;

public sealed record RenderOutletState(
    string OutletId,
    string SurfaceId,
    AovKind SourceOutput,
    PresentationMode PresentationMode);
