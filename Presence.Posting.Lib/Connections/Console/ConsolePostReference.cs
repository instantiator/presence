using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.Console;

public class ConsolePostReference : INetworkPostReference
{
    public string ReferenceKey { get; init; } = Guid.NewGuid().ToString();

    public SocialNetwork Network => SocialNetwork.Console;

    public CommonPost? Origin { get; init; }
}