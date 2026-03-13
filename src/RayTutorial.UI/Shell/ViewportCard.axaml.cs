using Avalonia;
using Avalonia.Controls;

namespace RayTutorial.UI.Shell;

public sealed partial class ViewportCard : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<string> SubtitleProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(Subtitle), string.Empty);

    public static readonly StyledProperty<string> BodyProperty =
        AvaloniaProperty.Register<ViewportCard, string>(nameof(Body), string.Empty);

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
}
