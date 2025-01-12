using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.Posting.Lib.Connections.Test;

public class TestPostReference : INetworkPostReference
{
    public string ReferenceKey { get; init; } = Guid.NewGuid().ToString();

    public SocialNetwork Network => SocialNetwork.Console;

    public CommonPost? Origin { get; init; }
}