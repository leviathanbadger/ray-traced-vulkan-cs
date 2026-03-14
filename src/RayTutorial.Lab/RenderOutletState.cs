using RayTutorial.Domain;

namespace RayTutorial.Lab;

public sealed record RenderOutletState(
    string OutletId,
    string SurfaceId,
    AovKind SelectedAov);
