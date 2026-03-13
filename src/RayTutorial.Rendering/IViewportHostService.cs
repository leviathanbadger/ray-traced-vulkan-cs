namespace RayTutorial.Rendering;

public interface IViewportHostService : IDisposable
{
    Task<ViewportHostStatus> AttachAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken);

    Task<ViewportHostStatus> ResizeAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken);

    Task<ViewportHostStatus> RenderFrameAsync(string viewportId, CancellationToken cancellationToken);

    Task DetachAsync(string viewportId, CancellationToken cancellationToken);
}
