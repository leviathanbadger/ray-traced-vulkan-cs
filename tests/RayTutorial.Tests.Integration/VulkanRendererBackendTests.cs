using RayTutorial.Rendering;
using RayTutorial.Rendering.Vulkan;
using RayTutorial.Scene;

namespace RayTutorial.Tests.Integration;

public sealed class VulkanRendererBackendTests
{
    [Fact]
    public async Task RendererBackendSupportsViewportLifecycle()
    {
        var renderer = new VulkanRendererBackend();

        await renderer.InitializeAsync(CancellationToken.None);
        await renderer.LoadSceneAsync(
            new SceneDescriptor(
                "PrimitiveDiagnostics",
                "PrimitiveDiagnostics",
                "Simple primitives and a few instances for first-hit inspection and debug overlays.",
                RayTutorial.Domain.CoordinateSystem.HoudiniStyle),
            CancellationToken.None);
        await renderer.AttachViewportAsync(
            "beauty",
            new NativeSurfaceDescriptor(123, "win32", new ViewportBounds(0, 0, 640, 360), new ViewportSize(640, 360)),
            CancellationToken.None);
        await renderer.ResizeViewportAsync(
            "beauty",
            new NativeSurfaceDescriptor(123, "win32", new ViewportBounds(0, 0, 800, 450), new ViewportSize(800, 450)),
            CancellationToken.None);

        var frame = await renderer.RenderFrameAsync("beauty", CancellationToken.None);

        Assert.Equal("beauty", frame.ViewportId);
        Assert.Contains("800x450", frame.Detail);
        Assert.Contains("PrimitiveDiagnostics", frame.Detail);
        Assert.Contains("win32", frame.Detail);

        await renderer.DetachViewportAsync("beauty", CancellationToken.None);
    }
}
