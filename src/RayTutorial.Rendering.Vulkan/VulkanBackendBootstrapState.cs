namespace RayTutorial.Rendering.Vulkan;

internal sealed record VulkanBackendBootstrapState(
    string InstanceName,
    string DeviceName,
    string SurfaceBackend,
    bool SupportsPresentation)
{
    public static VulkanBackendBootstrapState CreatePlaceholder() =>
        new("vk-placeholder-instance", "vk-placeholder-device", "win32", true);
}
