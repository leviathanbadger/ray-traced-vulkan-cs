using System.Collections.ObjectModel;

namespace RayTutorial.UI.Shell;

public sealed class ShellViewModel
{
    public ShellViewModel()
    {
        LessonSummaries = new ObservableCollection<LessonSummary>
        {
            new("Module 1", "Ray Queries and Visibility", "Inspect primary hits, normals, instance IDs, and the data returned by a ray query."),
            new("Module 2", "Acceleration Structures", "Compare instancing layouts, BLAS/TLAS responsibilities, and the intuition behind traversal cost."),
            new("Module 5", "From Single Bounce to Path Tracing", "Move from direct lighting into indirect transport, accumulation, and visible noise."),
            new("Module 9", "Offline and VFX Architecture", "Relate the live renderer to AOV-heavy offline workflows and production tradeoffs.")
        };

        ControlGroups = new ObservableCollection<ControlGroup>
        {
            new(
                "Integrator",
                "Core path and hybrid rendering knobs for the active lesson state.",
                new ObservableCollection<DialDescriptor>
                {
                    new("Max Bounces", 3),
                    new("Samples Per Pixel", 8),
                    new("Shadow Ray Budget", 32)
                }),
            new(
                "Materials",
                "Controls chosen to make the current lesson difference visually obvious.",
                new ObservableCollection<DialDescriptor>
                {
                    new("Surface Roughness", 35),
                    new("Metallic Weight", 70),
                    new("Emission Boost", 20)
                }),
            new(
                "Acceleration Structures",
                "Placeholder controls for BLAS/TLAS build strategy, instance count, and debug overlays.",
                new ObservableCollection<DialDescriptor>
                {
                    new("Instance Count", 48),
                    new("Refit Preference", 60),
                    new("Traversal Overlay", 15)
                })
        };

        SceneOptions = new ObservableCollection<string>
        {
            "PrimitiveDiagnostics",
            "CornellVariant",
            "GlossyInterior"
        };

        LayoutOptions = new ObservableCollection<string>
        {
            "Single Pane",
            "Split View",
            "Quad View"
        };

        AovOptions = new ObservableCollection<string>
        {
            "Beauty",
            "Normal",
            "Albedo",
            "Variance",
            "InstanceId"
        };

        SelectedScene = SceneOptions[1];
        SelectedLayout = LayoutOptions[2];
        SelectedAov = AovOptions[0];
        SelectedLesson = LessonSummaries[0];
    }

    public ObservableCollection<string> SceneOptions { get; }

    public ObservableCollection<string> LayoutOptions { get; }

    public ObservableCollection<string> AovOptions { get; }

    public ObservableCollection<LessonSummary> LessonSummaries { get; }

    public ObservableCollection<ControlGroup> ControlGroups { get; }

    public string SelectedScene { get; set; }

    public string SelectedLayout { get; set; }

    public string SelectedAov { get; set; }

    public LessonSummary SelectedLesson { get; set; }

    public string ActiveLessonHeadline =>
        "The shell is structured around guided experiments: select a lesson, load a preset, inspect the resulting AOVs, then compare pane states.";

    public string ActiveSimplificationNote =>
        "Early shell and lesson placeholders flatten a lot of renderer complexity. For example, the eventual Vulkan ray tracing pipeline and shader binding table setup will be represented through guided actions instead of raw low-level state.";

    public string StatusSummary =>
        "Phase 2 shell scaffold: UI regions are live placeholders waiting for rendering and lesson-state plumbing.";
}

public sealed record LessonSummary(string Module, string Title, string Summary);

public sealed record ControlGroup(string Title, string Summary, ObservableCollection<DialDescriptor> Controls);

public sealed class DialDescriptor
{
    public DialDescriptor(string label, double value)
    {
        Label = label;
        Value = value;
    }

    public string Label { get; }

    public double Value { get; set; }

    public string ValueLabel => $"{Value:0}";
}
