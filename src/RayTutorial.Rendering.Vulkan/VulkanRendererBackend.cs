using RayTutorial.Rendering;

namespace RayTutorial.Rendering.Vulkan;

public sealed class VulkanRendererBackend
{
    public const string BackendName = "Vulkan";

    public static Type RendererContract => typeof(IRenderer);
}
