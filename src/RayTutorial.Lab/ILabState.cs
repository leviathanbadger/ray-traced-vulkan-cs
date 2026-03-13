using System.ComponentModel;
using RayTutorial.Domain;

namespace RayTutorial.Lab;

public interface ILabState : INotifyPropertyChanged
{
    string SelectedLessonId { get; set; }

    string SelectedSceneId { get; set; }

    string SelectedLayoutName { get; set; }

    AovKind SelectedAov { get; set; }

    void ApplyPreset(LabPreset preset);
}
