using RayTutorial.Domain;
using RayTutorial.Scene;

namespace RayTutorial.Rendering;

public interface IRenderer
{
    string Name { get; }

    IReadOnlyCollection<AovKind> SupportedAovs { get; }

    ValueTask InitializeAsync(CancellationToken cancellationToken);

    ValueTask LoadSceneAsync(SceneDescriptor scene, CancellationToken cancellationToken);

    ValueTask ConfigureRenderSurfaceAsync(RenderSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken);

    ValueTask AttachRenderOutletAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken);

    ValueTask ResizeRenderOutletAsync(RenderOutletDescriptor outletDescriptor, CancellationToken cancellationToken);

    ValueTask<RenderFrameResult> RenderFrameAsync(string outletId, CancellationToken cancellationToken);

    ValueTask DetachRenderOutletAsync(string outletId, CancellationToken cancellationToken);
}
