using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.Posting.Lib.Connections.Console;

public class ConsoleConnection : INetworkConnection
{
    public SocialNetwork Network => SocialNetwork.Console;

    public INetworkCredentials? Credentials => null;

    public bool Connected => true;

    public async Task<bool> ConnectAsync(INetworkCredentials? credentials)
    {
        return true;
    }

    public void Disconnect()
    {
    }

    public void Dispose()
    {
        Disconnect();
    }

    public async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null)
    {
        var key = Guid.NewGuid().ToString();
        System.Console.WriteLine($"{key}: {(replyTo != null ? $"(reply to: {replyTo.ReferenceKey}) " : "")}{post.ComposeText()}");
        return new ConsolePostReference
        {
            ReferenceKey = key,
            Origin = post
        };
    }
}