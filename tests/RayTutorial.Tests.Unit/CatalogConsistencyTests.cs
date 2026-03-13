using RayTutorial.Lab;
using RayTutorial.Lessons;
using RayTutorial.Scene;

namespace RayTutorial.Tests.Unit;

public sealed class CatalogConsistencyTests
{
    [Fact]
    public void TutorialPresetCatalogReferencesKnownLessonsAndScenes()
    {
        var lessons = new TutorialLessonCatalog().GetLessons().Select(lesson => lesson.Id).ToHashSet();
        var scenes = new TutorialSceneCatalog().GetScenes().Select(scene => scene.Id).ToHashSet();
        var presets = new TutorialLabPresetCatalog().GetPresets();

        Assert.NotEmpty(presets);
        Assert.All(presets, preset =>
        {
            Assert.Contains(preset.LessonId, lessons);
            Assert.Contains(preset.SceneId, scenes);
        });
    }
}
