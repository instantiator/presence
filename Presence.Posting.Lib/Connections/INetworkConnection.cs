using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public interface INetworkConnection : IAsyncDisposable
{
    public SocialNetwork Network { get; }
    public string Prefix { get; }
    public bool Connected { get; }
    public Task<bool> ConnectAsync();
    public Task DisconnectAsync();
    public Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = default);
    public Task<IEnumerable<INetworkPostReference>> PostAsync(IEnumerable<CommonPost> thread);
    public Task<bool> DeletePostAsync(INetworkPostReference reference);
}