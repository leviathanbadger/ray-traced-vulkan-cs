using System.ComponentModel;

namespace RayTutorial.Rendering;

public interface ISceneSelectionState : INotifyPropertyChanged
{
    string SelectedSceneId { get; }

    RenderResolution SharedRenderResolution { get; }

    string GetRenderSurfaceId(string outletId);

    RenderSurfaceDescriptor GetRenderSurfaceDescriptor(string surfaceId);
}
