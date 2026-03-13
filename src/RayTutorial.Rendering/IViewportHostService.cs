namespace RayTutorial.Rendering;

public interface IViewportHostService
{
    Task<ViewportHostStatus> InitializeAsync(string viewportId, CancellationToken cancellationToken);
}
