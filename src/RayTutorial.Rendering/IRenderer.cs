using RayTutorial.Domain;
using RayTutorial.Scene;

namespace RayTutorial.Rendering;

public interface IRenderer
{
    string Name { get; }

    IReadOnlyCollection<AovKind> SupportedAovs { get; }

    ValueTask InitializeAsync(CancellationToken cancellationToken);

    ValueTask LoadSceneAsync(SceneDescriptor scene, CancellationToken cancellationToken);

    ValueTask AttachViewportAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken);

    ValueTask ResizeViewportAsync(string viewportId, NativeSurfaceDescriptor surfaceDescriptor, CancellationToken cancellationToken);

    ValueTask<RenderFrameResult> RenderFrameAsync(string viewportId, CancellationToken cancellationToken);

    ValueTask DetachViewportAsync(string viewportId, CancellationToken cancellationToken);
}
