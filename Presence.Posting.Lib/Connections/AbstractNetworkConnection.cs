using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.Posting.Lib.Connections;

public abstract class AbstractNetworkConnection : INetworkConnection
{
    public abstract SocialNetwork Network { get; }
    public INetworkCredentials? Credentials { get; protected set; }
    public abstract bool Connected { get; }
    protected bool CredentialsRequired { get; }
    public DateTime? LastAction { get; protected set; }

    public async Task<bool> ConnectAsync(INetworkCredentials? credentials = null)
    {
        if (CredentialsRequired)
        {
            if (credentials == null) throw new NullReferenceException($"Credentials are required.");
        }

        if (Credentials != null)
        {
            var (valid, errors) = credentials!.Validate();
            if (!valid) { throw new ArgumentException(string.Join(", ", errors)); }
        }

        this.Credentials = credentials ?? this.Credentials;
        await RateLimitAsync();
        return await ConnectImplementationAsync(credentials);
    }

    protected async Task RateLimitAsync()
    {
        await RateLimitImplementationAsync(LastAction);
        LastAction = DateTime.Now;
    }

    protected virtual async Task RateLimitImplementationAsync(DateTime? lastAction)
    {
    }

    protected abstract Task<bool> ConnectImplementationAsync(INetworkCredentials? credentials);
    
    public void Disconnect()
    {
        DisconnectImplementationAsync().Wait();
    }

    protected abstract Task DisconnectImplementationAsync();

    public void Dispose()
    {
        Disconnect();
    }

    public abstract Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null);

    public abstract Task<bool> DeletePostAsync(INetworkPostReference uri);
}