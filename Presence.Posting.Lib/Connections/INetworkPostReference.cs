using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public interface INetworkPostReference
{
    public string ReferenceKey { get; }
    public SocialNetwork Network { get; }
    public CommonPost? Origin { get; }
}