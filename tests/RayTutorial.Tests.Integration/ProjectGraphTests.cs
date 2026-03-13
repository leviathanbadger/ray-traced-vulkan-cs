using RayTutorial.App;
using RayTutorial.Rendering.Vulkan;

namespace RayTutorial.Tests.Integration;

public sealed class ProjectGraphTests
{
    [Fact]
    public void AppAndVulkanProjectsAreReachable()
    {
        Assert.NotNull(typeof(VulkanRendererBackend));
        Assert.NotNull(typeof(Program));
    }
}
