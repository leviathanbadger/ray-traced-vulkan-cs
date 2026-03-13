namespace RayTutorial.Rendering;

public readonly record struct NativeSurfaceDescriptor(
    nint WindowHandle,
    string HandleKind,
    ViewportBounds ViewportBounds,
    ViewportSize ViewportSize);
