using RayTutorial.Scene;

namespace RayTutorial.Assets;

public sealed record AssetManifest(
    string SceneId,
    string SourcePath,
    SceneDescriptor Scene);
