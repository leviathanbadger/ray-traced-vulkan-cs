using RayTutorial.Rendering;

namespace RayTutorial.Rendering.Vulkan;

internal sealed record VulkanSurfaceRuntimeState(
    RenderSurfaceDescriptor Descriptor,
    VulkanSurfaceResources Resources,
    int Generation,
    int AccumulatedFrames,
    bool WasReconfigured)
{
    public static VulkanSurfaceRuntimeState Create(RenderSurfaceDescriptor descriptor) =>
        new(descriptor, VulkanSurfaceResources.Create(descriptor), 1, 0, true);

    public VulkanSurfaceRuntimeState Reconfigure(RenderSurfaceDescriptor descriptor)
    {
        var requiresReset =
            Descriptor.SceneId != descriptor.SceneId
            || Descriptor.Resolution != descriptor.Resolution
            || Descriptor.RenderMode != descriptor.RenderMode
            || Descriptor.Quality != descriptor.Quality
            || !Descriptor.EnabledOutputs.SequenceEqual(descriptor.EnabledOutputs);

        if (!requiresReset)
        {
            return this with { Descriptor = descriptor, WasReconfigured = false };
        }

        return new VulkanSurfaceRuntimeState(
            descriptor,
            VulkanSurfaceResources.Create(descriptor),
            Generation + 1,
            0,
            true);
    }

    public VulkanSurfaceRuntimeState AdvanceFrame() =>
        this with
        {
            AccumulatedFrames = AccumulatedFrames + 1,
            WasReconfigured = false
        };
}
