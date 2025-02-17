namespace Presence.Posting.Lib.Connections;

public enum NetworkPostNotificationType 
{
    ImageUpload
}

public enum NetworkPostNotificationSeverity
{
    /// <summary>
    /// Something expected, that the user will want to know about, has happened.
    /// </summary>
    Info,
    /// <summary>
    /// Something will lead to an undesired outcome, but the application was able to continue.
    /// </summary>
    Warning,
    /// <summary>
    /// Something unexpected has happened which may affect the success of this operation. The application was able to continue.
    /// </summary>
    Error,
}

public class NetworkPostNotification
{
    public string? Message { get; init; }
    public NetworkPostNotificationType NotificationType { get; init; }
    public NetworkPostNotificationSeverity Severity { get; init; }
}