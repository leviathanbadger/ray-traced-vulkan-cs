using Avalonia.Controls;
using RayTutorial.Domain;
using RayTutorial.Lab;
using RayTutorial.Rendering;
using System.Globalization;
using System.ComponentModel;

namespace RayTutorial.UI.Shell;

public sealed partial class ShellWindow : Window
{
    private readonly IViewportHostService? viewportHostService;
    private readonly ShellViewModel shellViewModel;
    private readonly ILabState? labState;

    public ShellWindow()
        : this(new ShellViewModel(), null)
    {
    }

    public ShellWindow(ShellViewModel viewModel, IViewportHostService? viewportHostService)
        : this(viewModel, null, viewportHostService)
    {
    }

    public ShellWindow(ShellViewModel viewModel, ILabState? labState, IViewportHostService? viewportHostService)
    {
        shellViewModel = viewModel;
        this.labState = labState;
        this.viewportHostService = viewportHostService;
        DataContext = viewModel;
        InitializeComponent();
        BeautyViewport.HostService = viewportHostService;
        ComparisonViewport.HostService = viewportHostService;
        AovViewport.HostService = viewportHostService;
        PerformanceViewport.HostService = viewportHostService;
        BeautyViewport.SelectedAovChanged += OnViewportSelectedAovChanged;
        ComparisonViewport.SelectedAovChanged += OnViewportSelectedAovChanged;
        AovViewport.SelectedAovChanged += OnViewportSelectedAovChanged;
        PerformanceViewport.SelectedAovChanged += OnViewportSelectedAovChanged;
        BeautyViewport.PresentationChanged += OnViewportPresentationChanged;
        ComparisonViewport.PresentationChanged += OnViewportPresentationChanged;
        AovViewport.PresentationChanged += OnViewportPresentationChanged;
        PerformanceViewport.PresentationChanged += OnViewportPresentationChanged;
        BeautyViewport.ActionRequested += OnViewportActionRequested;
        ComparisonViewport.ActionRequested += OnViewportActionRequested;
        AovViewport.ActionRequested += OnViewportActionRequested;
        PerformanceViewport.ActionRequested += OnViewportActionRequested;
        BeautyViewport.SurfaceSettingsRequested += OnViewportSurfaceSettingsRequested;
        ComparisonViewport.SurfaceSettingsRequested += OnViewportSurfaceSettingsRequested;
        AovViewport.SurfaceSettingsRequested += OnViewportSurfaceSettingsRequested;
        PerformanceViewport.SurfaceSettingsRequested += OnViewportSurfaceSettingsRequested;
        SinglePaneButton.Click += (_, _) => shellViewModel.SelectedLayout = "Single Pane";
        SplitPaneButton.Click += (_, _) => shellViewModel.SelectedLayout = "Split View";
        QuadPaneButton.Click += (_, _) => shellViewModel.SelectedLayout = "Quad View";
        shellViewModel.PropertyChanged += OnShellViewModelPropertyChanged;
        this.labState?.PropertyChanged += OnLabStatePropertyChanged;
        SyncViewportCardsFromState();
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
        if (labState is not null)
        {
            labState.PropertyChanged -= OnLabStatePropertyChanged;
        }

        viewportHostService?.Dispose();
    }

    private void OnLabStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ILabState.RenderOutlets) or nameof(ILabState.SelectedSceneId))
        {
            SyncViewportCardsFromState();
        }
    }

    private void OnViewportSelectedAovChanged(object? sender, EventArgs e)
    {
        if (labState is null || sender is not ViewportCard card)
        {
            return;
        }

        if (Enum.TryParse<AovKind>(card.SelectedAov, out var selectedAov))
        {
            labState.EnsureOutputAvailableForOutlet(card.ViewId, selectedAov);
            labState.SetSelectedSourceOutput(card.ViewId, selectedAov);
        }
    }

    private void OnViewportPresentationChanged(object? sender, EventArgs e)
    {
        if (labState is null || sender is not ViewportCard card)
        {
            return;
        }

        if (Enum.TryParse<PresentationMode>(card.SelectedPresentation, out var presentationMode))
        {
            labState.SetPresentationMode(card.ViewId, presentationMode);
        }
    }

    private void OnViewportActionRequested(object? sender, ViewportActionRequestedEventArgs e)
    {
        if (labState is null)
        {
            return;
        }

        switch (e.ActionId)
        {
            case "compare-raw-vs-denoised":
                var sourceSurfaceId = labState.GetRenderSurfaceId(e.ViewportId);
                var sourceOutput = labState.GetSelectedSourceOutput(e.ViewportId);
                labState.BindOutletToSurface("comparison", sourceSurfaceId);
                labState.SetSelectedSourceOutput(e.ViewportId, sourceOutput);
                labState.SetPresentationMode(e.ViewportId, PresentationMode.Raw);
                labState.SetSelectedSourceOutput("comparison", sourceOutput);
                labState.SetPresentationMode("comparison", PresentationMode.Denoised);
                break;
            case "clone-surface":
                labState.ForkSurfaceForOutlet(e.ViewportId, "clone");
                break;
            case "inspect-tlas":
                labState.SetSelectedSourceOutput(e.ViewportId, AovKind.InstanceId);
                labState.SetPresentationMode(e.ViewportId, PresentationMode.Raw);
                break;
            case "enable-output":
                if (Enum.TryParse<AovKind>(e.ActionValue, out var outputToEnable))
                {
                    labState.EnsureOutputAvailableForOutlet(e.ViewportId, outputToEnable);
                    labState.SetSelectedSourceOutput(e.ViewportId, outputToEnable);
                    labState.SetPresentationMode(e.ViewportId, PresentationMode.Raw);
                }
                break;
        }
    }

    private void OnViewportSurfaceSettingsRequested(object? sender, ViewportSurfaceSettingsRequestedEventArgs e)
    {
        if (labState is null)
        {
            return;
        }

        labState.ApplySurfaceSettingsToOutlet(e.ViewportId, e.RenderMode, e.SamplesPerPixel, e.MaxBounces);
    }

    private void SyncViewportCardsFromState()
    {
        if (labState is null)
        {
            return;
        }

        SyncViewportCard(BeautyViewport);
        SyncViewportCard(ComparisonViewport);
        SyncViewportCard(AovViewport);
        SyncViewportCard(PerformanceViewport);
    }

    private void SyncViewportCard(ViewportCard card)
    {
        if (labState is null)
        {
            return;
        }

        var surfaceId = labState.GetRenderSurfaceId(card.ViewId);
        var surfaceDescriptor = labState.GetRenderSurfaceDescriptor(surfaceId);
        var surfaceState = labState.GetRenderSurfaceState(surfaceId);

        card.RenderSurfaceId = surfaceId;
        card.AovOptions = surfaceDescriptor.EnabledOutputs.Select(output => output.ToString()).ToArray();
        var availableOutputOptions = Enum.GetValues<AovKind>()
            .Where(output => !surfaceDescriptor.EnabledOutputs.Contains(output))
            .Select(output => output.ToString())
            .ToArray();
        card.AvailableOutputOptions = availableOutputOptions;
        card.SelectedOutputToEnable = availableOutputOptions.Length > 0 ? availableOutputOptions[0] : null;
        card.SelectedAov = labState.GetSelectedSourceOutput(card.ViewId).ToString();
        card.SelectedPresentation = labState.GetPresentationMode(card.ViewId).ToString();
        card.SelectedRenderMode = surfaceState.RenderMode;
        card.SelectedSamplesPerPixel = surfaceState.SamplesPerPixel.ToString(CultureInfo.InvariantCulture);
        card.SelectedMaxBounces = surfaceState.MaxBounces.ToString(CultureInfo.InvariantCulture);
        card.SurfaceStatus = surfaceId == "lesson-main" ? "Shared Surface" : "Forked Surface";
        card.EnabledOutputsSummary = BuildEnabledOutputsSummary(surfaceState);
    }

    private static string BuildEnabledOutputsSummary(RenderSurfaceState surfaceState)
    {
        var outputs = string.Join(", ", surfaceState.EnabledOutputs.Select(FormatAovLabel));
        return $"{FormatRenderModeLabel(surfaceState.RenderMode)} | {surfaceState.SamplesPerPixel} spp | {surfaceState.MaxBounces} bounces | {outputs}";
    }

    private static string FormatAovLabel(AovKind output)
    {
        return output switch
        {
            AovKind.InstanceId => "Instance ID",
            _ => string.Concat(
                output
                    .ToString()
                    .Select((character, index) => index > 0 && char.IsUpper(character) ? $" {character}" : character.ToString()))
        };
    }

    private static string FormatRenderModeLabel(RenderMode renderMode) =>
        string.Concat(
            renderMode.ToString().Select((character, index) => index > 0 && char.IsUpper(character) ? $" {character}" : character.ToString()));
}
