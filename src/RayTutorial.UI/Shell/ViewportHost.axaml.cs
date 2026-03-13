using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using RayTutorial.Rendering;

namespace RayTutorial.UI.Shell;

public sealed partial class ViewportHost : UserControl, IDisposable
{
    public static readonly StyledProperty<string> ViewportIdProperty =
        AvaloniaProperty.Register<ViewportHost, string>(nameof(ViewportId), string.Empty);

    public static readonly StyledProperty<IViewportHostService?> HostServiceProperty =
        AvaloniaProperty.Register<ViewportHost, IViewportHostService?>(nameof(HostService));

    public static readonly StyledProperty<string> StatusTitleProperty =
        AvaloniaProperty.Register<ViewportHost, string>(nameof(StatusTitle), "Preparing renderer host...");

    public static readonly StyledProperty<string> StatusDetailProperty =
        AvaloniaProperty.Register<ViewportHost, string>(nameof(StatusDetail), "Renderer setup and viewport lifecycle will initialize here.");

    private CancellationTokenSource? initializationCancellation;

    public ViewportHost()
    {
        InitializeComponent();
        AttachedToVisualTree += OnAttachedToVisualTree;
        DetachedFromVisualTree += OnDetachedFromVisualTree;
    }

    public string ViewportId
    {
        get => GetValue(ViewportIdProperty);
        set => SetValue(ViewportIdProperty, value);
    }

    public IViewportHostService? HostService
    {
        get => GetValue(HostServiceProperty);
        set => SetValue(HostServiceProperty, value);
    }

    public string StatusTitle
    {
        get => GetValue(StatusTitleProperty);
        set => SetValue(StatusTitleProperty, value);
    }

    public string StatusDetail
    {
        get => GetValue(StatusDetailProperty);
        set => SetValue(StatusDetailProperty, value);
    }

    private async void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (HostService is null || initializationCancellation is not null)
        {
            return;
        }

        initializationCancellation = new CancellationTokenSource();

        try
        {
            var status = await HostService.InitializeAsync(ViewportId, initializationCancellation.Token);
            StatusTitle = status.Title;
            StatusDetail = status.Detail;
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            initializationCancellation?.Dispose();
            initializationCancellation = null;
        }
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        initializationCancellation?.Cancel();
    }

    public void Dispose()
    {
        initializationCancellation?.Cancel();
        initializationCancellation?.Dispose();
        initializationCancellation = null;
    }
}
