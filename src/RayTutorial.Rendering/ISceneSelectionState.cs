using System.ComponentModel;
using RayTutorial.Domain;

namespace RayTutorial.Rendering;

public interface ISceneSelectionState : INotifyPropertyChanged
{
    string SelectedSceneId { get; }

    RenderResolution SharedRenderResolution { get; }

    string GetRenderSurfaceId(string outletId);

    RenderSurfaceDescriptor GetRenderSurfaceDescriptor(string surfaceId);

    bool IsOutputAvailable(string surfaceId, AovKind output);
}
