namespace RayTutorial.Rendering;

public sealed record RenderOutletDescriptor(
    string OutletId,
    string SurfaceId,
    NativeSurfaceDescriptor NativeSurface);
