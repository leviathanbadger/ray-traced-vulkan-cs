using RayTutorial.Lab;
using RayTutorial.Lessons;
using RayTutorial.Scene;
using RayTutorial.UI.Shell;

namespace RayTutorial.App;

internal sealed class AppCompositionRoot
{
    public static ShellWindow CreateShellWindow()
    {
        var lessonCatalog = new TutorialLessonCatalog();
        var sceneCatalog = new TutorialSceneCatalog();
        var presetCatalog = new TutorialLabPresetCatalog();

        var shellViewModel = new ShellViewModel(lessonCatalog, sceneCatalog, presetCatalog);
        return new ShellWindow(shellViewModel);
    }
}
