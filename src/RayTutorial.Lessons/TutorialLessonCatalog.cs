using RayTutorial.Domain;

namespace RayTutorial.Lessons;

public sealed class TutorialLessonCatalog : ILessonCatalog
{
    private static readonly IReadOnlyList<LessonDescriptor> Lessons =
    [
        new(
            "ray-queries-and-visibility",
            "Module 1",
            "Ray Queries and Visibility",
            "Inspect primary hits, normals, instance IDs, and the data returned by a ray query.",
            [AovKind.Beauty, AovKind.Normal, AovKind.Depth, AovKind.InstanceId],
            ["This lesson compresses hit shader details into a smaller teaching surface before the real Vulkan pipeline is exposed."]),
        new(
            "acceleration-structures",
            "Module 2",
            "Acceleration Structures",
            "Compare instancing layouts, BLAS/TLAS responsibilities, and the intuition behind traversal cost.",
            [AovKind.Beauty, AovKind.InstanceId, AovKind.Depth],
            ["Traversal and build heuristics are simplified here to teach the role of BLAS and TLAS before backend-specific details."]),
        new(
            "single-bounce-to-path-tracing",
            "Module 5",
            "From Single Bounce to Path Tracing",
            "Move from direct lighting into indirect transport, accumulation, and visible noise.",
            [AovKind.Beauty, AovKind.DirectDiffuse, AovKind.IndirectDiffuse, AovKind.Variance],
            ["The first lessons flatten integrator details and present a smaller set of controls than a real path tracer exposes."]),
        new(
            "offline-and-vfx-architecture",
            "Module 9",
            "Offline and VFX Architecture",
            "Relate the live renderer to AOV-heavy offline workflows and production tradeoffs.",
            [AovKind.Beauty, AovKind.Albedo, AovKind.Normal, AovKind.Emission],
            ["Production renderer comparisons stay architectural and intentionally avoid vendor-specific implementation claims."])
    ];

    public IReadOnlyList<LessonDescriptor> GetLessons() => Lessons;
}
