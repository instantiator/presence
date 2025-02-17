using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.Console;

public class ConsolePostReference : INetworkPostReference
{
    public IDictionary<string, string?> NetworkReferences { get; init; } = new Dictionary<string, string?>()
    {
        { "guid", Guid.NewGuid().ToString() }
    };

    public SocialNetwork Network => SocialNetwork.Console;
    public CommonPost? Origin { get; init; }
    public string? Link => "(printed to console)";
    public IEnumerable<NetworkPostNotification> Notifications { get; private set; } = new List<NetworkPostNotification>();
}