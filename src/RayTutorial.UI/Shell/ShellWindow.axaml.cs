using Avalonia.Controls;

namespace RayTutorial.UI.Shell;

public sealed partial class ShellWindow : Window
{
    public ShellWindow()
    {
        DataContext = new ShellViewModel();
        InitializeComponent();
    }
}
