using RayTutorial.Domain;

namespace RayTutorial.Lab;

public sealed record LabPreset(
    string Id,
    string LessonId,
    string DisplayName,
    string Description,
    string SceneId,
    string LayoutName,
    AovKind DefaultAov);
