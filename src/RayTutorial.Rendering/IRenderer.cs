using RayTutorial.Domain;
using RayTutorial.Scene;

namespace RayTutorial.Rendering;

public interface IRenderer
{
    string Name { get; }

    IReadOnlyCollection<AovKind> SupportedAovs { get; }

    void LoadScene(SceneDescriptor scene);
}
