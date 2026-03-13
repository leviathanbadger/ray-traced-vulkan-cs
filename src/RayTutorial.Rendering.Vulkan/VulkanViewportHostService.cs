using RayTutorial.Rendering;

namespace RayTutorial.Rendering.Vulkan;

public sealed class VulkanViewportHostService : IViewportHostService
{
    public async Task<ViewportHostStatus> InitializeAsync(string viewportId, CancellationToken cancellationToken)
    {
        return await Task.Run(async () =>
        {
            // Placeholder for future CPU-heavy renderer bootstrap work.
            await Task.Delay(120, cancellationToken);

            return new ViewportHostStatus(
                true,
                "Viewport Host Ready",
                $"{VulkanRendererBackend.BackendName} host seam is active for {viewportId}. Swapchain and frame loop are next.");
        }, cancellationToken);
    }
}
