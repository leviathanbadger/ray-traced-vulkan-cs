using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RayTutorial.Domain;
using RayTutorial.Lab;
using RayTutorial.Lessons;
using RayTutorial.Scene;

namespace RayTutorial.UI.Shell;

public sealed class ShellViewModel : INotifyPropertyChanged
{
    private readonly ILabState labState;
    private readonly Dictionary<string, LessonDescriptor> lessonsById;
    private readonly Dictionary<string, SceneDescriptor> scenesById;
    private readonly Dictionary<string, LabPreset> presetsByLessonId;

    public ShellViewModel()
        : this(new TutorialLessonCatalog(), new TutorialSceneCatalog(), new TutorialLabPresetCatalog(), new LabState())
    {
    }

    public ShellViewModel(
        ILessonCatalog lessonCatalog,
        ISceneCatalog sceneCatalog,
        ILabPresetCatalog presetCatalog,
        ILabState labState)
    {
        this.labState = labState;
        var lessons = lessonCatalog.GetLessons();
        var scenes = sceneCatalog.GetScenes();
        var presets = presetCatalog.GetPresets();

        lessonsById = lessons.ToDictionary(lesson => lesson.Id);
        scenesById = scenes.ToDictionary(scene => scene.Id);
        presetsByLessonId = presets.ToDictionary(preset => preset.LessonId);

        LessonSummaries = new ObservableCollection<LessonSummary>(
            lessons.Select(lesson => new LessonSummary(lesson.Id, lesson.Module, lesson.Title, lesson.Summary)));

        SceneOptions = new ObservableCollection<string>(scenes.Select(scene => scene.DisplayName));
        LayoutOptions = new ObservableCollection<string>(["Single Pane", "Split View", "Quad View"]);
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

        var startupPreset = presets.Count > 0
            ? presets[0]
            : throw new InvalidOperationException("At least one lab preset is required.");
        labState.ApplyPreset(startupPreset);
        labState.PropertyChanged += OnLabStatePropertyChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<string> SceneOptions { get; }

    public ObservableCollection<string> LayoutOptions { get; }

    public ObservableCollection<LessonSummary> LessonSummaries { get; }

    public ObservableCollection<ControlGroup> ControlGroups { get; }

    public string SelectedScene
    {
        get => labState.SelectedSceneId;
        set
        {
            if (labState.SelectedSceneId != value)
            {
                labState.SelectedSceneId = value;
            }
        }
    }

    public string SelectedLayout
    {
        get => labState.SelectedLayoutName;
        set
        {
            if (labState.SelectedLayoutName != value)
            {
                labState.SelectedLayoutName = value;
            }
        }
    }

    public LessonSummary SelectedLesson
    {
        get => LessonSummaries.First(summary => summary.Id == labState.SelectedLessonId);
        set
        {
            if (labState.SelectedLessonId == value.Id)
            {
                return;
            }

            ApplyLessonDefaults(value.Id);
        }
    }

    public string ActiveLessonHeadline => GetSelectedLesson().Summary;

    public string ActiveSimplificationNote => GetSelectedLesson().SimplificationNotes[0];

    public string StatusSummary =>
        $"Phase 2 shell scaffold: lesson, scene, and preset catalogs are now wired; rendering and live lab state are next.";

    private LessonDescriptor GetSelectedLesson() => lessonsById[labState.SelectedLessonId];

    private void ApplyLessonDefaults(string lessonId)
    {
        var preset = GetPresetForLesson(lessonId);
        labState.ApplyPreset(preset);
    }

    private LabPreset GetPresetForLesson(string lessonId)
    {
        return presetsByLessonId.TryGetValue(lessonId, out var preset)
            ? preset
            : throw new InvalidOperationException($"No preset registered for lesson '{lessonId}'.");
    }

    private void OnLabStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ILabState.SelectedLessonId):
                OnPropertyChanged(nameof(SelectedLesson));
                OnPropertyChanged(nameof(ActiveLessonHeadline));
                OnPropertyChanged(nameof(ActiveSimplificationNote));
                break;
            case nameof(ILabState.SelectedSceneId):
                OnPropertyChanged(nameof(SelectedScene));
                break;
            case nameof(ILabState.SelectedLayoutName):
                OnPropertyChanged(nameof(SelectedLayout));
                break;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed record LessonSummary(string Id, string Module, string Title, string Summary);

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
