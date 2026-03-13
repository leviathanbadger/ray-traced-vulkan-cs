namespace RayTutorial.Rendering;

public interface IViewportHostService
{
    Task<ViewportHostStatus> AttachAsync(string viewportId, ViewportSize viewportSize, CancellationToken cancellationToken);

    Task<ViewportHostStatus> ResizeAsync(string viewportId, ViewportSize viewportSize, CancellationToken cancellationToken);

    Task<ViewportHostStatus> RenderFrameAsync(string viewportId, CancellationToken cancellationToken);

    Task DetachAsync(string viewportId, CancellationToken cancellationToken);
}
