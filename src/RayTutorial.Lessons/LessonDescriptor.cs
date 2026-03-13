using RayTutorial.Domain;

namespace RayTutorial.Lessons;

public sealed record LessonDescriptor(
    string Id,
    string Title,
    string Summary,
    IReadOnlyList<AovKind> RecommendedAovs,
    IReadOnlyList<string> SimplificationNotes);
