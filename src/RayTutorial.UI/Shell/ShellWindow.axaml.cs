using Avalonia.Controls;
using RayTutorial.Rendering;

namespace RayTutorial.UI.Shell;

public sealed partial class ShellWindow : Window
{
    public ShellWindow()
        : this(new ShellViewModel(), null)
    {
    }

    public ShellWindow(ShellViewModel viewModel, IViewportHostService? viewportHostService)
    {
        DataContext = viewModel;
        InitializeComponent();
        BeautyViewport.HostService = viewportHostService;
        ComparisonViewport.HostService = viewportHostService;
        AovViewport.HostService = viewportHostService;
        PerformanceViewport.HostService = viewportHostService;
        Opened += (_, _) => WindowsChrome.TryApply(this);
    }
}
