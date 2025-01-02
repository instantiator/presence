using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.Posting.Lib.Connections;

public interface INetworkConnection : IDisposable
{
    public SocialNetwork Network { get; }
    
    public string Server { get; }
    public string Account { get; }
    public bool Authenticated { get; }

    public Task<bool> ConnectAsync();
    public void Disconnect();

    public Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null);
}