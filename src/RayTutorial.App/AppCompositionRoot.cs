using RayTutorial.Lab;
using RayTutorial.Lessons;
using RayTutorial.Rendering;
using RayTutorial.Rendering.Vulkan;
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
        var labState = new LabState();
        var renderer = new VulkanRendererBackend();
        IViewportHostService viewportHostService = new VulkanViewportHostService(renderer, labState, sceneCatalog);

        var shellViewModel = new ShellViewModel(lessonCatalog, sceneCatalog, presetCatalog, labState);
        return new ShellWindow(shellViewModel, viewportHostService);
    }
}
