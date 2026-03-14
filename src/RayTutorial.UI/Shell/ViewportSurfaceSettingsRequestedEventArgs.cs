using RayTutorial.Rendering;

namespace RayTutorial.UI.Shell;

public sealed class ViewportSurfaceSettingsRequestedEventArgs : EventArgs
{
    public ViewportSurfaceSettingsRequestedEventArgs(string viewportId, RenderMode renderMode, int samplesPerPixel, int maxBounces)
    {
        ViewportId = viewportId;
        RenderMode = renderMode;
        SamplesPerPixel = samplesPerPixel;
        MaxBounces = maxBounces;
    }

    public string ViewportId { get; }

    public RenderMode RenderMode { get; }

    public int SamplesPerPixel { get; }

    public int MaxBounces { get; }
}
