using Avalonia.Controls;
using RayTutorial.Rendering;
using System.ComponentModel;

namespace RayTutorial.UI.Shell;

public sealed partial class ShellWindow : Window
{
    private readonly IViewportHostService? viewportHostService;
    private readonly ShellViewModel shellViewModel;

    public ShellWindow()
        : this(new ShellViewModel(), null)
    {
    }

    public ShellWindow(ShellViewModel viewModel, IViewportHostService? viewportHostService)
    {
        shellViewModel = viewModel;
        this.viewportHostService = viewportHostService;
        DataContext = viewModel;
        InitializeComponent();
        BeautyViewport.HostService = viewportHostService;
        ComparisonViewport.HostService = viewportHostService;
        AovViewport.HostService = viewportHostService;
        PerformanceViewport.HostService = viewportHostService;
        SinglePaneButton.Click += (_, _) => shellViewModel.SelectedLayout = "Single Pane";
        SplitPaneButton.Click += (_, _) => shellViewModel.SelectedLayout = "Split View";
        QuadPaneButton.Click += (_, _) => shellViewModel.SelectedLayout = "Quad View";
        shellViewModel.PropertyChanged += OnShellViewModelPropertyChanged;
        ApplyViewportLayout(shellViewModel.SelectedLayout);
        Opened += (_, _) => WindowsChrome.TryApply(this);
        Closed += OnClosed;
    }

    private void OnShellViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ShellViewModel.SelectedLayout))
        {
            ApplyViewportLayout(shellViewModel.SelectedLayout);
        }
    }

    private void ApplyViewportLayout(string layoutName)
    {
        ResetPaneLayout();

        switch (layoutName)
        {
            case "Single Pane":
                BeautyPane.IsVisible = true;
                BeautyPane.SetValue(Grid.ColumnSpanProperty, 2);
                BeautyPane.SetValue(Grid.RowSpanProperty, 2);
                ComparisonPane.IsVisible = false;
                AovPane.IsVisible = false;
                PerformancePane.IsVisible = false;
                break;
            case "Split View":
                BeautyPane.IsVisible = true;
                ComparisonPane.IsVisible = true;
                BeautyPane.SetValue(Grid.RowSpanProperty, 2);
                ComparisonPane.SetValue(Grid.RowSpanProperty, 2);
                AovPane.IsVisible = false;
                PerformancePane.IsVisible = false;
                break;
            default:
                BeautyPane.IsVisible = true;
                ComparisonPane.IsVisible = true;
                AovPane.IsVisible = true;
                PerformancePane.IsVisible = true;
                break;
        }
    }

    private void ResetPaneLayout()
    {
        BeautyPane.IsVisible = true;
        ComparisonPane.IsVisible = true;
        AovPane.IsVisible = true;
        PerformancePane.IsVisible = true;
        BeautyPane.SetValue(Grid.ColumnSpanProperty, 1);
        BeautyPane.SetValue(Grid.RowSpanProperty, 1);
        ComparisonPane.SetValue(Grid.RowSpanProperty, 1);
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        shellViewModel.PropertyChanged -= OnShellViewModelPropertyChanged;
        viewportHostService?.Dispose();
    }
}
