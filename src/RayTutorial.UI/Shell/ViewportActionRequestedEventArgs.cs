namespace RayTutorial.UI.Shell;

public sealed class ViewportActionRequestedEventArgs : EventArgs
{
    public ViewportActionRequestedEventArgs(string viewportId, string actionId)
    {
        ViewportId = viewportId;
        ActionId = actionId;
    }

    public string ViewportId { get; }

    public string ActionId { get; }
}
