using RayTutorial.Rendering;

namespace RayTutorial.Rendering.Vulkan;

internal sealed record VulkanSurfaceResources(
    string SurfaceId,
    RenderResolution Resolution,
    RenderMode RenderMode,
    RenderQualitySettings Quality,
    IReadOnlyList<string> OutputAttachments)
{
    public static VulkanSurfaceResources Create(RenderSurfaceDescriptor descriptor) =>
        new(
            descriptor.SurfaceId,
            descriptor.Resolution,
            descriptor.RenderMode,
            descriptor.Quality,
            descriptor.EnabledOutputs.Select(output => output.ToString()).ToArray());
}
