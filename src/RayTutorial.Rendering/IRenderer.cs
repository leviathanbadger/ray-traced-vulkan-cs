using RayTutorial.Domain;
using RayTutorial.Scene;

namespace RayTutorial.Rendering;

public interface IRenderer
{
    string Name { get; }

    IReadOnlyCollection<AovKind> SupportedAovs { get; }

    ValueTask InitializeAsync(CancellationToken cancellationToken);

    ValueTask AttachViewportAsync(string viewportId, ViewportSize viewportSize, CancellationToken cancellationToken);

    ValueTask ResizeViewportAsync(string viewportId, ViewportSize viewportSize, CancellationToken cancellationToken);

    ValueTask<RenderFrameResult> RenderFrameAsync(string viewportId, CancellationToken cancellationToken);

    ValueTask DetachViewportAsync(string viewportId, CancellationToken cancellationToken);

    void LoadScene(SceneDescriptor scene);
}
