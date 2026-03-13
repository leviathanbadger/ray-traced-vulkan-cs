using RayTutorial.Rendering;
using RayTutorial.Rendering.Vulkan;

namespace RayTutorial.Tests.Integration;

public sealed class VulkanRendererBackendTests
{
    [Fact]
    public async Task RendererBackendSupportsViewportLifecycle()
    {
        var renderer = new VulkanRendererBackend();

        await renderer.InitializeAsync(CancellationToken.None);
        await renderer.AttachViewportAsync("beauty", new ViewportSize(640, 360), CancellationToken.None);
        await renderer.ResizeViewportAsync("beauty", new ViewportSize(800, 450), CancellationToken.None);

        var frame = await renderer.RenderFrameAsync("beauty", CancellationToken.None);

        Assert.Equal("beauty", frame.ViewportId);
        Assert.Contains("800x450", frame.Detail);

        await renderer.DetachViewportAsync("beauty", CancellationToken.None);
    }
}
