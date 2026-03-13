using RayTutorial.Domain;

namespace RayTutorial.Lab;

public sealed class TutorialLabPresetCatalog : ILabPresetCatalog
{
    private static readonly IReadOnlyList<LabPreset> Presets =
    [
        new(
            "ray-queries-default",
            "ray-queries-and-visibility",
            "Primitive visibility debug",
            "Sets up the primitive diagnostics scene with a quad layout and hit-inspection AOVs.",
            "PrimitiveDiagnostics",
            "Quad View",
            AovKind.Beauty),
        new(
            "accel-structures-default",
            "acceleration-structures",
            "TLAS inspection",
            "Sets up an instancing-oriented comparison layout for structure discussion.",
            "PrimitiveDiagnostics",
            "Split View",
            AovKind.InstanceId),
        new(
            "path-tracing-default",
            "single-bounce-to-path-tracing",
            "Cornell path tracing",
            "Starts in the Cornell scene with variance-friendly defaults.",
            "CornellVariant",
            "Quad View",
            AovKind.Variance),
        new(
            "offline-architecture-default",
            "offline-and-vfx-architecture",
            "Offline workflow overview",
            "Starts from a beauty-first view with supporting diagnostic panes.",
            "GlossyInterior",
            "Split View",
            AovKind.Beauty)
    ];

    public IReadOnlyList<LabPreset> GetPresets() => Presets;
}
