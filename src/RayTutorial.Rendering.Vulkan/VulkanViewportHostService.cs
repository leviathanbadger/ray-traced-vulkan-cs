using System.ComponentModel;
using System.Threading;
using RayTutorial.Rendering;
using RayTutorial.Scene;

namespace RayTutorial.Rendering.Vulkan;

public sealed class VulkanViewportHostService : IViewportHostService
{
    private readonly IRenderer renderer;
    private readonly ISceneSelectionState sceneSelectionState;
    private readonly Dictionary<string, SceneDescriptor> scenesById;
    private readonly SemaphoreSlim sceneLoadGate = new(1, 1);

    public VulkanViewportHostService(
        IRenderer renderer,
        ISceneSelectionState sceneSelectionState,
        ISceneCatalog sceneCatalog)
    {
        this.renderer = renderer;
        this.sceneSelectionState = sceneSelectionState;
        scenesById = sceneCatalog.GetScenes().ToDictionary(scene => scene.Id);
        this.sceneSelectionState.PropertyChanged += OnLabStatePropertyChanged;
    }

    public VulkanViewportHostService(
        ISceneSelectionState sceneSelectionState,
        ISceneCatalog sceneCatalog)
        : this(new VulkanRendererBackend(), sceneSelectionState, sceneCatalog)
    {
    }

    public async Task<ViewportHostStatus> AttachAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken)
    {
        await EnsureSceneLoadedAsync(cancellationToken);
        await renderer.AttachViewportAsync(viewportId, surfaceDescriptor, cancellationToken);
        return new ViewportHostStatus(
            true,
            "Viewport Attached",
            $"{VulkanRendererBackend.BackendName} host attached at {surfaceDescriptor.ViewportSize.Width}x{surfaceDescriptor.ViewportSize.Height} on {surfaceDescriptor.HandleKind}.");
    }

    public async Task<ViewportHostStatus> ResizeAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken)
    {
        await renderer.ResizeViewportAsync(viewportId, surfaceDescriptor, cancellationToken);
        return new ViewportHostStatus(
            true,
            "Viewport Resized",
            $"Renderer host resized {viewportId} to {surfaceDescriptor.ViewportSize.Width}x{surfaceDescriptor.ViewportSize.Height}.");
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

    private async void OnLabStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ISceneSelectionState.SelectedSceneId))
        {
            return;
        }

        try
        {
            await EnsureSceneLoadedAsync(CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task EnsureSceneLoadedAsync(CancellationToken cancellationToken)
    {
        await sceneLoadGate.WaitAsync(cancellationToken);

        try
        {
            if (!scenesById.TryGetValue(sceneSelectionState.SelectedSceneId, out var scene))
            {
                return;
            }

            await renderer.LoadSceneAsync(scene, cancellationToken);
        }
        finally
        {
            sceneLoadGate.Release();
        }
    }

    public void Dispose()
    {
        sceneSelectionState.PropertyChanged -= OnLabStatePropertyChanged;
        sceneLoadGate.Dispose();
    }
}
