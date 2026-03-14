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

    public async Task<ViewportHostStatus> AttachAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken)
    {
        await EnsureSceneLoadedAsync(outletDescriptor.SurfaceId, cancellationToken);
        await renderer.AttachRenderOutletAsync(outletDescriptor, cancellationToken);
        return new ViewportHostStatus(
            true,
            "Outlet Attached",
            $"{VulkanRendererBackend.BackendName} outlet {outletDescriptor.OutletId} attached to shared surface {outletDescriptor.SurfaceId} with display size {outletDescriptor.NativeSurface.ViewportSize.Width}x{outletDescriptor.NativeSurface.ViewportSize.Height}.");
    }

    public async Task<ViewportHostStatus> ResizeAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken)
    {
        await renderer.ResizeRenderOutletAsync(outletDescriptor, cancellationToken);
        return new ViewportHostStatus(
            true,
            "Outlet Resized",
            $"Presentation outlet {outletDescriptor.OutletId} resized to {outletDescriptor.NativeSurface.ViewportSize.Width}x{outletDescriptor.NativeSurface.ViewportSize.Height}; shared render resolution remains fixed.");
    }

    public async Task<ViewportHostStatus> RenderFrameAsync(string outletId, CancellationToken cancellationToken)
    {
        var frame = await renderer.RenderFrameAsync(outletId, cancellationToken);
        return new ViewportHostStatus(true, frame.Title, frame.Detail);
    }

    public async Task DetachAsync(string outletId, CancellationToken cancellationToken)
    {
        await renderer.DetachRenderOutletAsync(outletId, cancellationToken);
    }

    private async void OnLabStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not (nameof(ISceneSelectionState.SelectedSceneId) or "RenderSurfaces" or "RenderOutlets" or nameof(ISceneSelectionState.SharedRenderResolution)))
        {
            return;
        }

        try
        {
            await EnsureSceneLoadedAsync(sceneSelectionState.GetRenderSurfaceId("beauty"), CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task EnsureSceneLoadedAsync(string surfaceId, CancellationToken cancellationToken)
    {
        await sceneLoadGate.WaitAsync(cancellationToken);

        try
        {
            if (!scenesById.TryGetValue(sceneSelectionState.SelectedSceneId, out var scene))
            {
                return;
            }

            await renderer.LoadSceneAsync(scene, cancellationToken);
            await renderer.ConfigureRenderSurfaceAsync(sceneSelectionState.GetRenderSurfaceDescriptor(surfaceId), cancellationToken);
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
