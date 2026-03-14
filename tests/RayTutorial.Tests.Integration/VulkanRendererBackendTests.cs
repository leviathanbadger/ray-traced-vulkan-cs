using RayTutorial.Rendering;
using RayTutorial.Rendering.Vulkan;
using RayTutorial.Scene;
using RayTutorial.Domain;

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
        await renderer.ConfigureRenderSurfaceAsync(
            new RenderSurfaceDescriptor(
                "lesson-main",
                "PrimitiveDiagnostics",
                new RenderResolution(1280, 720),
                "PathTracingPreview",
                8,
                3,
                [AovKind.Beauty, AovKind.Normal, AovKind.Variance, AovKind.InstanceId]),
            CancellationToken.None);
        await renderer.AttachRenderOutletAsync(
            new RenderOutletDescriptor(
                "beauty",
                "lesson-main",
                new NativeSurfaceDescriptor(123, "win32", new ViewportBounds(0, 0, 640, 360), new ViewportSize(640, 360))),
            CancellationToken.None);
        await renderer.ResizeRenderOutletAsync(
            new RenderOutletDescriptor(
                "beauty",
                "lesson-main",
                new NativeSurfaceDescriptor(123, "win32", new ViewportBounds(0, 0, 800, 450), new ViewportSize(800, 450))),
            CancellationToken.None);

        var frame = await renderer.RenderFrameAsync("beauty", CancellationToken.None);

        Assert.Equal("beauty", frame.OutletId);
        Assert.Equal("lesson-main", frame.SurfaceId);
        Assert.Contains("1280x720", frame.Detail);
        Assert.Contains("PrimitiveDiagnostics", frame.Detail);
        Assert.Contains("win32", frame.Detail);
        Assert.Contains("PathTracingPreview", frame.Detail);
        Assert.Contains("8 spp", frame.Detail);
        Assert.Contains("3 bounces", frame.Detail);
        Assert.Contains("surface generation 1", frame.Detail);
        Assert.Contains("accumulated frames 1", frame.Detail);

        await renderer.DetachRenderOutletAsync("beauty", CancellationToken.None);
    }

    [Fact]
    public async Task RendererBackendResetsAccumulationWhenSurfaceSettingsChange()
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
        await renderer.ConfigureRenderSurfaceAsync(
            new RenderSurfaceDescriptor(
                "lesson-main",
                "PrimitiveDiagnostics",
                new RenderResolution(1280, 720),
                "PathTracingPreview",
                8,
                3,
                [AovKind.Beauty, AovKind.Normal, AovKind.Variance, AovKind.InstanceId]),
            CancellationToken.None);
        await renderer.AttachRenderOutletAsync(
            new RenderOutletDescriptor(
                "beauty",
                "lesson-main",
                new NativeSurfaceDescriptor(123, "win32", new ViewportBounds(0, 0, 640, 360), new ViewportSize(640, 360))),
            CancellationToken.None);

        var firstFrame = await renderer.RenderFrameAsync("beauty", CancellationToken.None);
        await renderer.ConfigureRenderSurfaceAsync(
            new RenderSurfaceDescriptor(
                "lesson-main",
                "PrimitiveDiagnostics",
                new RenderResolution(1280, 720),
                "PathTracingReference",
                64,
                6,
                [AovKind.Beauty, AovKind.Normal, AovKind.Variance, AovKind.InstanceId]),
            CancellationToken.None);
        var secondFrame = await renderer.RenderFrameAsync("beauty", CancellationToken.None);

        Assert.Contains("surface generation 1", firstFrame.Detail);
        Assert.Contains("accumulated frames 1", firstFrame.Detail);
        Assert.Contains("surface generation 2", secondFrame.Detail);
        Assert.Contains("accumulated frames 1", secondFrame.Detail);
        Assert.Contains("PathTracingReference", secondFrame.Detail);
        Assert.Contains("64 spp", secondFrame.Detail);
        Assert.Contains("6 bounces", secondFrame.Detail);

        await renderer.DetachRenderOutletAsync("beauty", CancellationToken.None);
    }
}
