namespace RayTutorial.UI.Shell;

public sealed class ViewportActionRequestedEventArgs : EventArgs
{
    public ViewportActionRequestedEventArgs(string viewportId, string actionId, string? actionValue = null)
    {
        ViewportId = viewportId;
        ActionId = actionId;
        ActionValue = actionValue;
    }

    public string ViewportId { get; }

    public string ActionId { get; }

    public string? ActionValue { get; }
}
