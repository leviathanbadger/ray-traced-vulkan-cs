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

    IReadOnlyCollection<RenderSurfaceState> RenderSurfaces { get; }

    IReadOnlyCollection<RenderOutletState> RenderOutlets { get; }

    void ApplyPreset(LabPreset preset);

    AovKind GetSelectedAov(string outletId);

    void SetSelectedAov(string outletId, AovKind aov);

    void ForkSurfaceForOutlet(string outletId, string forkReason);
}
