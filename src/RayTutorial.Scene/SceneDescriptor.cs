using RayTutorial.Domain;

namespace RayTutorial.Scene;

public sealed record SceneDescriptor(
    string Id,
    string DisplayName,
    string Description,
    CoordinateSystem CoordinateSystem);
