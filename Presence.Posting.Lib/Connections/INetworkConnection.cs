using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public interface INetworkConnection : IDisposable
{
    public SocialNetwork Network { get; }
    public INetworkCredentials? Credentials { get; }
    public bool Connected { get; }
    public Task<bool> ConnectAsync(INetworkCredentials? credentials);
    public void Disconnect();
    public Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null);
    public Task<IEnumerable<INetworkPostReference>> PostAsync(IEnumerable<CommonPost> thread);
    public Task<bool> DeletePostAsync(INetworkPostReference uri);
}