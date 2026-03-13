using System.ComponentModel;
using System.Runtime.CompilerServices;
using RayTutorial.Domain;

namespace RayTutorial.Lab;

public sealed class LabState : ILabState
{
    private string selectedLessonId = string.Empty;
    private string selectedSceneId = string.Empty;
    private string selectedLayoutName = string.Empty;
    private AovKind selectedAov = AovKind.Beauty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string SelectedLessonId
    {
        get => selectedLessonId;
        set => SetProperty(ref selectedLessonId, value);
    }

    public string SelectedSceneId
    {
        get => selectedSceneId;
        set => SetProperty(ref selectedSceneId, value);
    }

    public string SelectedLayoutName
    {
        get => selectedLayoutName;
        set => SetProperty(ref selectedLayoutName, value);
    }

    public AovKind SelectedAov
    {
        get => selectedAov;
        set => SetProperty(ref selectedAov, value);
    }

    public void ApplyPreset(LabPreset preset)
    {
        SelectedLessonId = preset.LessonId;
        SelectedSceneId = preset.SceneId;
        SelectedLayoutName = preset.LayoutName;
        SelectedAov = preset.DefaultAov;
    }

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
