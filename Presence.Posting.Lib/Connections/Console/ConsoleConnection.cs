using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.Posting.Lib.Connections.Console;

public class ConsoleConnection : AbstractNetworkConnection
{
    public override SocialNetwork Network => SocialNetwork.Console;

    public override bool Connected => true;

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null)
    {
        var key = Guid.NewGuid().ToString();
        System.Console.WriteLine($"Post ({key}): {(replyTo != null ? $"(reply to: {replyTo.ReferenceKey}) " : "")}{post.ComposeText()}");
        return new ConsolePostReference
        {
            ReferenceKey = key,
            Origin = post
        };
    }

    public override async Task<bool> DeletePostAsync(INetworkPostReference uri)
    {
        System.Console.WriteLine($"Deletion ({uri.ReferenceKey})");
        return true;
    }

    protected override async Task<bool> ConnectImplementationAsync(INetworkCredentials? credentials)
    {
        System.Console.WriteLine($"Connect");
        return true;
    }

    protected override async Task DisconnectImplementationAsync()
    {
        System.Console.WriteLine($"Disconnect");
    }

}