using System.ComponentModel;
using RayTutorial.Domain;
using RayTutorial.Rendering;

namespace RayTutorial.Lab;

public interface ILabState : INotifyPropertyChanged, ISceneSelectionState
{
    string SelectedLessonId { get; set; }

    new string SelectedSceneId { get; set; }

    string SelectedLayoutName { get; set; }

    AovKind SelectedAov { get; set; }

    void ApplyPreset(LabPreset preset);
}
