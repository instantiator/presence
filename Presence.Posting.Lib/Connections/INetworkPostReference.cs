using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public interface INetworkPostReference
{
    public IDictionary<string, string?> NetworkReferences { get; }
    public SocialNetwork Network { get; }
    public CommonPost? Origin { get; }
    public string? Link { get; }
    public IEnumerable<NetworkPostNotification> Notifications { get; }
}