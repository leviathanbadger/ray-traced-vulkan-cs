using RayTutorial.Rendering;

namespace RayTutorial.UI.Shell;

public sealed class ViewportSurfaceSettingsRequestedEventArgs : EventArgs
{
    public ViewportSurfaceSettingsRequestedEventArgs(string viewportId, RenderMode renderMode, RenderQualitySettings quality)
    {
        ViewportId = viewportId;
        RenderMode = renderMode;
        Quality = quality;
    }

    public string ViewportId { get; }

    public RenderMode RenderMode { get; }

    public RenderQualitySettings Quality { get; }
}
