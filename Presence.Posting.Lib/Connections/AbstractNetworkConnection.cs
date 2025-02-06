using System.Diagnostics.CodeAnalysis;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public abstract class AbstractNetworkConnection : INetworkConnection
{
    [SetsRequiredMembers]
    protected AbstractNetworkConnection(INetworkAccount account)
    {
        Account = account;
    }

    public required INetworkAccount Account { get; init; }
    public SocialNetwork Network => Account.SocialNetwork;
    public string Prefix => Account.AccountPrefix;
    public abstract bool Connected { get; }
    public DateTime? LastAction { get; protected set; }

    public async Task<bool> ConnectAsync()
    {
        var (valid, errors) = Account!.Validate();
        if (!valid) { throw new ArgumentException(string.Join(", ", errors)); }
        await RateLimitAsync();
        return await ConnectImplementationAsync(Account);
    }

    protected async Task RateLimitAsync()
    {
        await RateLimitImplementationAsync(LastAction);
        LastAction = DateTime.Now;
    }

    protected virtual async Task RateLimitImplementationAsync(DateTime? lastAction)
    {
    }

    protected abstract Task<bool> ConnectImplementationAsync(INetworkAccount account);

    public void Disconnect()
    {
        DisconnectImplementationAsync().Wait();
    }

    protected abstract Task DisconnectImplementationAsync();

    public void Dispose()
    {
        Disconnect();
    }

    public abstract Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = default);

    public async Task<IEnumerable<INetworkPostReference>> PostAsync(IEnumerable<CommonPost> thread)
    {
        var references = new List<INetworkPostReference>();
        foreach (var post in thread)
        {
            var reference = await PostAsync(post, references.LastOrDefault());
            references.Add(reference);
        }
        return references;
    }

    public abstract Task<bool> DeletePostAsync(INetworkPostReference uri);
}