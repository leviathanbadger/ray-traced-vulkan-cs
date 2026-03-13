using System.Collections.Concurrent;
using RayTutorial.Domain;
using RayTutorial.Scene;

namespace RayTutorial.Rendering.Vulkan;

public sealed class VulkanRendererBackend : IRenderer
{
    private readonly ConcurrentDictionary<string, NativeSurfaceDescriptor> attachedViewports = new();
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

    public async ValueTask AttachViewportAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken)
    {
        await InitializeAsync(cancellationToken);
        await Task.Run(() => attachedViewports[viewportId] = surfaceDescriptor, cancellationToken);
    }

    public async ValueTask ResizeViewportAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken)
    {
        await Task.Run(() => attachedViewports[viewportId] = surfaceDescriptor, cancellationToken);
    }

    public async ValueTask<RenderFrameResult> RenderFrameAsync(string viewportId, CancellationToken cancellationToken)
    {
        return await Task.Run(async () =>
        {
            await Task.Delay(16, cancellationToken);

            attachedViewports.TryGetValue(viewportId, out var surfaceDescriptor);
            var loadedSceneName = loadedScene?.DisplayName ?? "No scene loaded";

            return new RenderFrameResult(
                viewportId,
                "Renderer Frame Ready",
                $"{BackendName} placeholder frame for {viewportId} at {surfaceDescriptor.ViewportSize.Width}x{surfaceDescriptor.ViewportSize.Height} using {loadedSceneName} on {surfaceDescriptor.HandleKind}.");
        }, cancellationToken);
    }

    public async ValueTask DetachViewportAsync(string viewportId, CancellationToken cancellationToken)
    {
        await Task.Run(() => attachedViewports.TryRemove(viewportId, out _), cancellationToken);
    }
}
