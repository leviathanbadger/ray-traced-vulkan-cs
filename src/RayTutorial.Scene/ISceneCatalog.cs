namespace RayTutorial.Scene;

public interface ISceneCatalog
{
    IReadOnlyList<SceneDescriptor> GetScenes();
}
