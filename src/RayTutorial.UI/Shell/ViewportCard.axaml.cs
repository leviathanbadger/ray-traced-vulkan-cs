using Avalonia;
using Avalonia.Controls;
using RayTutorial.Rendering;

namespace RayTutorial.UI.Shell;

public sealed partial class ViewportCard : UserControl
{
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

    public ViewportCard()
    {
        InitializeComponent();
    }

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
}
