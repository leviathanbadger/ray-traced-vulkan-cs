using System.Collections.Concurrent;
using RayTutorial.Domain;
using RayTutorial.Scene;

namespace RayTutorial.Rendering.Vulkan;

public sealed class VulkanRendererBackend : IRenderer
{
    private readonly ConcurrentDictionary<string, RenderSurfaceDescriptor> renderSurfaces = new();
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
        await Task.Run(() => renderSurfaces[surfaceDescriptor.SurfaceId] = surfaceDescriptor, cancellationToken);
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
            renderSurfaces.TryGetValue(outletDescriptor?.SurfaceId ?? string.Empty, out var renderSurface);
            var loadedSceneName = loadedScene?.DisplayName ?? "No scene loaded";
            var resolution = renderSurface?.Resolution ?? RenderResolution.Default;
            var handleKind = outletDescriptor?.NativeSurface.HandleKind ?? "unknown";
            var renderMode = renderSurface?.RenderMode ?? "UnknownMode";
            var samplesPerPixel = renderSurface?.SamplesPerPixel ?? 0;
            var maxBounces = renderSurface?.MaxBounces ?? 0;
            var enabledOutputs = renderSurface is null
                ? "none"
                : string.Join(", ", renderSurface.EnabledOutputs);

            return new RenderFrameResult(
                outletId,
                renderSurface?.SurfaceId ?? "unbound",
                "Renderer Frame Ready",
                $"{BackendName} placeholder frame for {outletId} presenting shared surface {renderSurface?.SurfaceId ?? "unbound"} at render resolution {resolution.Width}x{resolution.Height} using {loadedSceneName} on {handleKind}; mode {renderMode}, {samplesPerPixel} spp, {maxBounces} bounces, outputs: {enabledOutputs}.");
        }, cancellationToken);
    }

    public async ValueTask DetachRenderOutletAsync(string outletId, CancellationToken cancellationToken)
    {
        await Task.Run(() => attachedOutlets.TryRemove(outletId, out _), cancellationToken);
    }
}
