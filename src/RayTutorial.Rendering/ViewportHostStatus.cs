namespace RayTutorial.Rendering;

public sealed record ViewportHostStatus(
    bool IsReady,
    string Title,
    string Detail);
