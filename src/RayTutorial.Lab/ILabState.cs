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

    RenderSurfaceState GetRenderSurfaceState(string surfaceId);

    void ApplyPreset(LabPreset preset);

    AovKind GetSelectedSourceOutput(string outletId);

    PresentationMode GetPresentationMode(string outletId);

    void SetSelectedSourceOutput(string outletId, AovKind output);

    void SetPresentationMode(string outletId, PresentationMode presentationMode);

    void EnsureOutputAvailableForOutlet(string outletId, AovKind output);

    void ApplySurfaceSettingsToOutlet(string outletId, RenderMode renderMode, int samplesPerPixel, int maxBounces);

    void BindOutletToSurface(string outletId, string surfaceId);

    void ForkSurfaceForOutlet(string outletId, string forkReason);
}
