namespace RayTutorial.Rendering;

public interface IViewportHostService : IDisposable
{
    Task<ViewportHostStatus> AttachAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken);

    Task<ViewportHostStatus> ResizeAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken);

    Task<ViewportHostStatus> RenderFrameAsync(string outletId, CancellationToken cancellationToken);

    Task DetachAsync(string outletId, CancellationToken cancellationToken);
}
