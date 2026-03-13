using Avalonia.Controls;
using RayTutorial.Rendering;

namespace RayTutorial.UI.Shell;

public sealed partial class ShellWindow : Window
{
    private readonly IViewportHostService? viewportHostService;

    public ShellWindow()
        : this(new ShellViewModel(), null)
    {
    }

    public ShellWindow(ShellViewModel viewModel, IViewportHostService? viewportHostService)
    {
        this.viewportHostService = viewportHostService;
        DataContext = viewModel;
        InitializeComponent();
        BeautyViewport.HostService = viewportHostService;
        ComparisonViewport.HostService = viewportHostService;
        AovViewport.HostService = viewportHostService;
        PerformanceViewport.HostService = viewportHostService;
        Opened += (_, _) => WindowsChrome.TryApply(this);
        Closed += (_, _) => this.viewportHostService?.Dispose();
    }
}
