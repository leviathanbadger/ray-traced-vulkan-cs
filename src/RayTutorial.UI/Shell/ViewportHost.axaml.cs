using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
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
    private DispatcherTimer? renderTimer;
    private bool renderInFlight;

    public ViewportHost()
    {
        InitializeComponent();
        AttachedToVisualTree += OnAttachedToVisualTree;
        DetachedFromVisualTree += OnDetachedFromVisualTree;
        SizeChanged += OnSizeChanged;
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
            var status = await HostService.AttachAsync(ViewportId, GetViewportSize(), initializationCancellation.Token);
            StatusTitle = status.Title;
            StatusDetail = status.Detail;
            StartRenderLoop();
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
        }
    }

    private async void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        StopRenderLoop();

        if (HostService is not null && initializationCancellation is not null)
        {
            try
            {
                await HostService.DetachAsync(ViewportId, initializationCancellation.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        initializationCancellation?.Cancel();
        initializationCancellation?.Dispose();
        initializationCancellation = null;
    }

    public void Dispose()
    {
        StopRenderLoop();
        initializationCancellation?.Cancel();
        initializationCancellation?.Dispose();
        initializationCancellation = null;
    }

    private async void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (HostService is null || initializationCancellation is null || GetViewportSize().IsEmpty)
        {
            return;
        }

        try
        {
            var status = await HostService.ResizeAsync(ViewportId, GetViewportSize(), initializationCancellation.Token);
            StatusTitle = status.Title;
            StatusDetail = status.Detail;
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void StartRenderLoop()
    {
        if (renderTimer is not null)
        {
            return;
        }

        renderTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(250)
        };
        renderTimer.Tick += OnRenderTimerTick;
        renderTimer.Start();
    }

    private void StopRenderLoop()
    {
        if (renderTimer is null)
        {
            return;
        }

        renderTimer.Stop();
        renderTimer.Tick -= OnRenderTimerTick;
        renderTimer = null;
    }

    private async void OnRenderTimerTick(object? sender, EventArgs e)
    {
        if (HostService is null || initializationCancellation is null || renderInFlight)
        {
            return;
        }

        renderInFlight = true;

        try
        {
            var status = await HostService.RenderFrameAsync(ViewportId, initializationCancellation.Token);
            StatusTitle = status.Title;
            StatusDetail = status.Detail;
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            renderInFlight = false;
        }
    }

    private ViewportSize GetViewportSize()
    {
        var width = Math.Max(1, (int)Bounds.Width);
        var height = Math.Max(1, (int)Bounds.Height);
        return new ViewportSize(width, height);
    }
}
