using Avalonia.Controls;

namespace RayTutorial.UI.Shell;

public sealed partial class ShellWindow : Window
{
    public ShellWindow()
        : this(new ShellViewModel())
    {
    }

    public ShellWindow(ShellViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        Opened += (_, _) => WindowsChrome.TryApply(this);
    }
}
