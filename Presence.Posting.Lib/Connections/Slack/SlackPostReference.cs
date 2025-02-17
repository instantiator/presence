using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.Slack;

public class SlackWebhookPostReference : INetworkPostReference
{
    public SlackWebhookPostReference(string? ts, CommonPost? origin, IEnumerable<NetworkPostNotification>? notifications = null)
    {
        Origin = origin;
        Ts = ts;
        Notifications = notifications ?? new List<NetworkPostNotification>();
    }

    public string? Ts { get; private set; }

    public IDictionary<string, string?> NetworkReferences => new Dictionary<string, string?>()
    {
        { "ts", Ts }
    };

    public SocialNetwork Network => SocialNetwork.SlackWebhook;

    public CommonPost? Origin { get; private set; }

    public string? Link => null;

    public IEnumerable<NetworkPostNotification> Notifications { get; private set; }
}