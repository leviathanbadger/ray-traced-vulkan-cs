namespace RayTutorial.UI.Shell;

public sealed class ViewportSurfaceSettingsRequestedEventArgs : EventArgs
{
    public ViewportSurfaceSettingsRequestedEventArgs(string viewportId, string renderMode, int samplesPerPixel, int maxBounces)
    {
        ViewportId = viewportId;
        RenderMode = renderMode;
        SamplesPerPixel = samplesPerPixel;
        MaxBounces = maxBounces;
    }

    public string ViewportId { get; }

    public string RenderMode { get; }

    public int SamplesPerPixel { get; }

    public int MaxBounces { get; }
}
