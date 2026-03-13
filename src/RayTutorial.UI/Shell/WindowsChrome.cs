using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Controls.Platform;

namespace RayTutorial.UI.Shell;

internal static class WindowsChrome
{
    private const uint DwmwaWindowCornerPreference = 33;
    private const uint DwmwaUseImmersiveDarkMode = 20;
    private const uint DwcpRound = 2;

    public static void TryApply(Window window)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var platformHandle = window.TryGetPlatformHandle();
        if (platformHandle?.Handle is not nint hwnd || hwnd == 0)
        {
            return;
        }

        var roundedCorners = DwcpRound;
        _ = DwmSetWindowAttribute(hwnd, DwmwaWindowCornerPreference, ref roundedCorners, sizeof(uint));

        var darkMode = 1;
        _ = DwmSetWindowAttribute(hwnd, DwmwaUseImmersiveDarkMode, ref darkMode, sizeof(int));
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(
        nint hwnd,
        uint attribute,
        ref uint value,
        int attributeSize);

    [DllImport("dwmapi.dll", EntryPoint = "DwmSetWindowAttribute")]
    private static extern int DwmSetWindowAttribute(
        nint hwnd,
        uint attribute,
        ref int value,
        int attributeSize);
}
