using System.Collections.Concurrent;
using RayTutorial.Domain;
using RayTutorial.Scene;

namespace RayTutorial.Rendering.Vulkan;

public sealed class VulkanRendererBackend : IRenderer
{
    private readonly ConcurrentDictionary<string, VulkanSurfaceState> renderSurfaces = new();
    private readonly ConcurrentDictionary<string, RenderOutletDescriptor> attachedOutlets = new();
    private SceneDescriptor? loadedScene;
    private bool initialized;

    public const string BackendName = "Vulkan";

    public string Name => BackendName;

    public IReadOnlyCollection<AovKind> SupportedAovs { get; } =
    [
        AovKind.Beauty,
        AovKind.Albedo,
        AovKind.Normal,
        AovKind.Depth,
        AovKind.InstanceId,
        AovKind.Variance
    ];

    public async ValueTask InitializeAsync(CancellationToken cancellationToken)
    {
        if (initialized)
        {
            return;
        }

        await Task.Run(async () =>
        {
            await Task.Delay(120, cancellationToken);
            initialized = true;
        }, cancellationToken);
    }

    public async ValueTask LoadSceneAsync(SceneDescriptor scene, CancellationToken cancellationToken)
    {
        await Task.Run(() => loadedScene = scene, cancellationToken);
    }

    public async ValueTask ConfigureRenderSurfaceAsync(RenderSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken)
    {
        await InitializeAsync(cancellationToken);
        await Task.Run(() =>
        {
            renderSurfaces.AddOrUpdate(
                surfaceDescriptor.SurfaceId,
                surfaceId => VulkanSurfaceState.Create(surfaceDescriptor),
                (_, existingState) => existingState.Reconfigure(surfaceDescriptor));
        }, cancellationToken);
    }

    public async ValueTask AttachRenderOutletAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken)
    {
        await Task.Run(() => attachedOutlets[outletDescriptor.OutletId] = outletDescriptor, cancellationToken);
    }

    public async ValueTask ResizeRenderOutletAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken)
    {
        await Task.Run(() => attachedOutlets[outletDescriptor.OutletId] = outletDescriptor, cancellationToken);
    }

    public async ValueTask<RenderFrameResult> RenderFrameAsync(string outletId, CancellationToken cancellationToken)
    {
        return await Task.Run(async () =>
        {
            await Task.Delay(16, cancellationToken);

            attachedOutlets.TryGetValue(outletId, out var outletDescriptor);
            renderSurfaces.TryGetValue(outletDescriptor?.SurfaceId ?? string.Empty, out var renderSurfaceState);
            if (renderSurfaceState is not null)
            {
                renderSurfaces[renderSurfaceState.Descriptor.SurfaceId] = renderSurfaceState = renderSurfaceState.AdvanceFrame();
            }

            var renderSurface = renderSurfaceState?.Descriptor;
            var loadedSceneName = loadedScene?.DisplayName ?? "No scene loaded";
            var resolution = renderSurface?.Resolution ?? RenderResolution.Default;
            var handleKind = outletDescriptor?.NativeSurface.HandleKind ?? "unknown";
            var renderMode = renderSurface?.RenderMode;
            var quality = renderSurface?.Quality ?? default;
            var enabledOutputs = renderSurface is null
                ? "none"
                : string.Join(", ", renderSurface.EnabledOutputs);
            var generation = renderSurfaceState?.Generation ?? 0;
            var accumulatedFrames = renderSurfaceState?.AccumulatedFrames ?? 0;
            var reconfigured = renderSurfaceState?.WasReconfigured ?? false;

            return new RenderFrameResult(
                outletId,
                renderSurface?.SurfaceId ?? "unbound",
                "Renderer Frame Ready",
                $"{BackendName} placeholder frame for {outletId} presenting shared surface {renderSurface?.SurfaceId ?? "unbound"} at render resolution {resolution.Width}x{resolution.Height} using {loadedSceneName} on {handleKind}; mode {(renderMode?.ToString() ?? "UnknownMode")}, {quality.SamplesPerPixel} spp, {quality.MaxBounces} bounces, outputs: {enabledOutputs}; surface generation {generation}, accumulated frames {accumulatedFrames}, reconfigured {reconfigured.ToString().ToLowerInvariant()}.");
        }, cancellationToken);
    }

    public async ValueTask DetachRenderOutletAsync(string outletId, CancellationToken cancellationToken)
    {
        await Task.Run(() => attachedOutlets.TryRemove(outletId, out _), cancellationToken);
    }

    private sealed record VulkanSurfaceState(
        RenderSurfaceDescriptor Descriptor,
        int Generation,
        int AccumulatedFrames,
        bool WasReconfigured)
    {
        public static VulkanSurfaceState Create(RenderSurfaceDescriptor descriptor) =>
            new(descriptor, 1, 0, true);

        public VulkanSurfaceState Reconfigure(RenderSurfaceDescriptor descriptor)
        {
            var requiresReset =
                Descriptor.SceneId != descriptor.SceneId
                || Descriptor.Resolution != descriptor.Resolution
                || Descriptor.RenderMode != descriptor.RenderMode
                || Descriptor.Quality != descriptor.Quality
                || !Descriptor.EnabledOutputs.SequenceEqual(descriptor.EnabledOutputs);

            if (!requiresReset)
            {
                return this with { Descriptor = descriptor, WasReconfigured = false };
            }

            return new VulkanSurfaceState(descriptor, Generation + 1, 0, true);
        }

        public VulkanSurfaceState AdvanceFrame() =>
            this with
            {
                AccumulatedFrames = AccumulatedFrames + 1,
                WasReconfigured = false
            };
    }
}
