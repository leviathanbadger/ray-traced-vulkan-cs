using RayTutorial.Rendering;
using RayTutorial.Scene;

namespace RayTutorial.Rendering.Vulkan;

public sealed class VulkanViewportHostService : IViewportHostService
{
    private readonly IRenderer renderer;

    public VulkanViewportHostService()
        : this(new VulkanRendererBackend())
    {
    }

    public VulkanViewportHostService(IRenderer renderer)
    {
        this.renderer = renderer;
        this.renderer.LoadScene(new SceneDescriptor(
            "PrimitiveDiagnostics",
            "PrimitiveDiagnostics",
            "Simple primitives and a few instances for first-hit inspection and debug overlays.",
            Domain.CoordinateSystem.HoudiniStyle));
    }

    public async Task<ViewportHostStatus> AttachAsync(string viewportId, ViewportSize viewportSize, CancellationToken cancellationToken)
    {
        await renderer.AttachViewportAsync(viewportId, viewportSize, cancellationToken);
        return new ViewportHostStatus(true, "Viewport Attached", $"{VulkanRendererBackend.BackendName} host attached at {viewportSize.Width}x{viewportSize.Height}.");
    }

    public async Task<ViewportHostStatus> ResizeAsync(string viewportId, ViewportSize viewportSize, CancellationToken cancellationToken)
    {
        await renderer.ResizeViewportAsync(viewportId, viewportSize, cancellationToken);
        return new ViewportHostStatus(true, "Viewport Resized", $"Renderer host resized {viewportId} to {viewportSize.Width}x{viewportSize.Height}.");
    }

    public async Task<ViewportHostStatus> RenderFrameAsync(string viewportId, CancellationToken cancellationToken)
    {
        var frame = await renderer.RenderFrameAsync(viewportId, cancellationToken);
        return new ViewportHostStatus(true, frame.Title, frame.Detail);
    }

    public async Task DetachAsync(string viewportId, CancellationToken cancellationToken)
    {
        await renderer.DetachViewportAsync(viewportId, cancellationToken);
    }
}
