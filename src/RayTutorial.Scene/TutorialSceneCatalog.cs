using RayTutorial.Domain;

namespace RayTutorial.Scene;

public sealed class TutorialSceneCatalog : ISceneCatalog
{
    private static readonly IReadOnlyList<SceneDescriptor> Scenes =
    [
        new(
            "PrimitiveDiagnostics",
            "PrimitiveDiagnostics",
            "Simple primitives and a few instances for first-hit inspection and debug overlays.",
            CoordinateSystem.HoudiniStyle),
        new(
            "CornellVariant",
            "CornellVariant",
            "Controlled bounce-lighting scene for path tracing, AOV comparison, and convergence checks.",
            CoordinateSystem.HoudiniStyle),
        new(
            "GlossyInterior",
            "GlossyInterior",
            "Reflective interior scene for roughness, variance, and multi-pane comparisons.",
            CoordinateSystem.HoudiniStyle)
    ];

    public IReadOnlyList<SceneDescriptor> GetScenes() => Scenes;
}
