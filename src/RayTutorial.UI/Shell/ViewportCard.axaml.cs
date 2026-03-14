using Avalonia;
using Avalonia.Controls;
using RayTutorial.Rendering;
using System.Collections.Generic;

namespace RayTutorial.UI.Shell;

public sealed partial class ViewportCard : UserControl
{
    private static readonly IReadOnlyList<string> DefaultAovOptions =
    [
        "Beauty",
        "Normal",
        "Albedo",
        "Variance",
        "InstanceId"
    ];

    private static readonly IReadOnlyList<string> DefaultPresentationOptions =
    [
        "Raw",
        "Denoised"
    ];

    private static readonly IReadOnlyList<string> DefaultAvailableOutputOptions =
    [
        "Depth",
        "WorldPosition",
        "DirectDiffuse",
        "IndirectDiffuse",
        "DirectSpecular",
        "IndirectSpecular",
        "Emission"
    ];

    private static readonly IReadOnlyList<RenderMode> DefaultRenderModeOptions =
    [
        RenderMode.PathTracingPreview,
        RenderMode.PathTracingReference,
        RenderMode.HybridRayQuery
    ];

    private static readonly IReadOnlyList<string> DefaultSamplesPerPixelOptions =
    [
        "1",
        "8",
        "32",
        "64"
    ];

    private static readonly IReadOnlyList<string> DefaultMaxBounceOptions =
    [
        "1",
        "3",
        "6",
        "8"
    ];

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<string> SubtitleProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(Subtitle), string.Empty);

    public static readonly StyledProperty<string> BodyProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(Body), string.Empty);

    public static readonly StyledProperty<string> ViewIdProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(ViewId), string.Empty);

    public static readonly StyledProperty<IViewportHostService?> HostServiceProperty =
        AvaloniaProperty.Register<ViewportCard, IViewportHostService?>(nameof(HostService));

    public static readonly StyledProperty<string> RenderSurfaceIdProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(RenderSurfaceId), "lesson-main");

    public static readonly StyledProperty<IReadOnlyList<string>> AovOptionsProperty =
        AvaloniaProperty.Register<ViewportCard, IReadOnlyList<string>>(nameof(AovOptions), DefaultAovOptions);

    public static readonly StyledProperty<string> SelectedAovProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(SelectedAov), "Beauty");

    public static readonly StyledProperty<IReadOnlyList<string>> PresentationOptionsProperty =
        AvaloniaProperty.Register<ViewportCard, IReadOnlyList<string>>(nameof(PresentationOptions), DefaultPresentationOptions);

    public static readonly StyledProperty<string> SelectedPresentationProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(SelectedPresentation), "Raw");

    public static readonly StyledProperty<string> SurfaceStatusProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(SurfaceStatus), "Shared Surface");

    public static readonly StyledProperty<string> EnabledOutputsSummaryProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(EnabledOutputsSummary), string.Empty);

    public static readonly StyledProperty<IReadOnlyList<string>> AvailableOutputOptionsProperty =
        AvaloniaProperty.Register<ViewportCard, IReadOnlyList<string>>(nameof(AvailableOutputOptions), DefaultAvailableOutputOptions);

    public static readonly StyledProperty<string?> SelectedOutputToEnableProperty =
        AvaloniaProperty.Register<ViewportCard, string?>(nameof(SelectedOutputToEnable));

    public static readonly StyledProperty<IReadOnlyList<RenderMode>> RenderModeOptionsProperty =
        AvaloniaProperty.Register<ViewportCard, IReadOnlyList<RenderMode>>(nameof(RenderModeOptions), DefaultRenderModeOptions);

    public static readonly StyledProperty<RenderMode> SelectedRenderModeProperty =
        AvaloniaProperty.Register<ViewportCard, RenderMode>(nameof(SelectedRenderMode), RenderMode.PathTracingPreview);

    public static readonly StyledProperty<IReadOnlyList<string>> SamplesPerPixelOptionsProperty =
        AvaloniaProperty.Register<ViewportCard, IReadOnlyList<string>>(nameof(SamplesPerPixelOptions), DefaultSamplesPerPixelOptions);

    public static readonly StyledProperty<string> SelectedSamplesPerPixelProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(SelectedSamplesPerPixel), "8");

    public static readonly StyledProperty<IReadOnlyList<string>> MaxBounceOptionsProperty =
        AvaloniaProperty.Register<ViewportCard, IReadOnlyList<string>>(nameof(MaxBounceOptions), DefaultMaxBounceOptions);

    public static readonly StyledProperty<string> SelectedMaxBouncesProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(SelectedMaxBounces), "3");

    public ViewportCard()
    {
        InitializeComponent();
        AovSelector.SelectionChanged += (_, _) => SelectedAovChanged?.Invoke(this, EventArgs.Empty);
        PresentationSelector.SelectionChanged += (_, _) => PresentationChanged?.Invoke(this, EventArgs.Empty);
        CompareActionButton.Click += (_, _) => ActionRequested?.Invoke(this, new ViewportActionRequestedEventArgs(ViewId, "compare-raw-vs-denoised"));
        InspectTlasActionButton.Click += (_, _) => ActionRequested?.Invoke(this, new ViewportActionRequestedEventArgs(ViewId, "inspect-tlas"));
        CloneSurfaceActionButton.Click += (_, _) => ActionRequested?.Invoke(this, new ViewportActionRequestedEventArgs(ViewId, "clone-surface"));
        EnableOutputButton.Click += (_, _) =>
        {
            if (!string.IsNullOrWhiteSpace(SelectedOutputToEnable))
            {
                ActionRequested?.Invoke(this, new ViewportActionRequestedEventArgs(ViewId, "enable-output", SelectedOutputToEnable));
            }
        };
        ApplySurfaceSettingsButton.Click += (_, _) =>
        {
            if (int.TryParse(SelectedSamplesPerPixel, out var samplesPerPixel)
                && int.TryParse(SelectedMaxBounces, out var maxBounces))
            {
                SurfaceSettingsRequested?.Invoke(
                    this,
                    new ViewportSurfaceSettingsRequestedEventArgs(
                        ViewId,
                        SelectedRenderMode,
                        new RenderQualitySettings(samplesPerPixel, maxBounces)));
            }
        };
    }

    public event EventHandler? SelectedAovChanged;

    public event EventHandler? PresentationChanged;

    public event EventHandler<ViewportActionRequestedEventArgs>? ActionRequested;

    public event EventHandler<ViewportSurfaceSettingsRequestedEventArgs>? SurfaceSettingsRequested;

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public string Body
    {
        get => GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    public string ViewId
    {
        get => GetValue(ViewIdProperty);
        set => SetValue(ViewIdProperty, value);
    }

    public IViewportHostService? HostService
    {
        get => GetValue(HostServiceProperty);
        set => SetValue(HostServiceProperty, value);
    }

    public string RenderSurfaceId
    {
        get => GetValue(RenderSurfaceIdProperty);
        set => SetValue(RenderSurfaceIdProperty, value);
    }

    public IReadOnlyList<string> AovOptions
    {
        get => GetValue(AovOptionsProperty);
        set => SetValue(AovOptionsProperty, value);
    }

    public string SelectedAov
    {
        get => GetValue(SelectedAovProperty);
        set => SetValue(SelectedAovProperty, value);
    }

    public IReadOnlyList<string> PresentationOptions
    {
        get => GetValue(PresentationOptionsProperty);
        set => SetValue(PresentationOptionsProperty, value);
    }

    public string SelectedPresentation
    {
        get => GetValue(SelectedPresentationProperty);
        set => SetValue(SelectedPresentationProperty, value);
    }

    public string SurfaceStatus
    {
        get => GetValue(SurfaceStatusProperty);
        set => SetValue(SurfaceStatusProperty, value);
    }

    public string EnabledOutputsSummary
    {
        get => GetValue(EnabledOutputsSummaryProperty);
        set => SetValue(EnabledOutputsSummaryProperty, value);
    }

    public IReadOnlyList<string> AvailableOutputOptions
    {
        get => GetValue(AvailableOutputOptionsProperty);
        set => SetValue(AvailableOutputOptionsProperty, value);
    }

    public string? SelectedOutputToEnable
    {
        get => GetValue(SelectedOutputToEnableProperty);
        set => SetValue(SelectedOutputToEnableProperty, value);
    }

    public IReadOnlyList<RenderMode> RenderModeOptions
    {
        get => GetValue(RenderModeOptionsProperty);
        set => SetValue(RenderModeOptionsProperty, value);
    }

    public RenderMode SelectedRenderMode
    {
        get => GetValue(SelectedRenderModeProperty);
        set => SetValue(SelectedRenderModeProperty, value);
    }

    public IReadOnlyList<string> SamplesPerPixelOptions
    {
        get => GetValue(SamplesPerPixelOptionsProperty);
        set => SetValue(SamplesPerPixelOptionsProperty, value);
    }

    public string SelectedSamplesPerPixel
    {
        get => GetValue(SelectedSamplesPerPixelProperty);
        set => SetValue(SelectedSamplesPerPixelProperty, value);
    }

    public IReadOnlyList<string> MaxBounceOptions
    {
        get => GetValue(MaxBounceOptionsProperty);
        set => SetValue(MaxBounceOptionsProperty, value);
    }

    public string SelectedMaxBounces
    {
        get => GetValue(SelectedMaxBouncesProperty);
        set => SetValue(SelectedMaxBouncesProperty, value);
    }
}
