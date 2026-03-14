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

    public ViewportCard()
    {
        InitializeComponent();
        AovSelector.SelectionChanged += (_, _) => SelectedAovChanged?.Invoke(this, EventArgs.Empty);
        CompareActionButton.Click += (_, _) => ActionRequested?.Invoke(this, new ViewportActionRequestedEventArgs(ViewId, "compare-raw-vs-denoised"));
        InspectTlasActionButton.Click += (_, _) => ActionRequested?.Invoke(this, new ViewportActionRequestedEventArgs(ViewId, "inspect-tlas"));
        CloneSurfaceActionButton.Click += (_, _) => ActionRequested?.Invoke(this, new ViewportActionRequestedEventArgs(ViewId, "clone-surface"));
    }

    public event EventHandler? SelectedAovChanged;

    public event EventHandler<ViewportActionRequestedEventArgs>? ActionRequested;

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
}
