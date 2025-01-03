using System.Diagnostics.CodeAnalysis;
using FishyFlip;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Richtext;
using FishyFlip.Lexicon.Com.Atproto.Repo;
using FishyFlip.Models;
using FishyFlip.Tools;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.Posting.Lib.Connections.AT;

public class ATConnection : INetworkConnection
{
    private const int RATE_ms = 1000;
    private static DateTime lastAction = DateTime.MinValue;

    public INetworkCredentials? Credentials { get; private set; }
    public Uri Server => Credentials?.ContainsKey(NetworkCredentialType.Server) == true ? new Uri("https://" + Credentials[NetworkCredentialType.Server]) : new Uri("https://bsky.social");
    private string? account => Credentials?[NetworkCredentialType.AccountName];
    private string? password => Credentials?[NetworkCredentialType.AppPassword];

    private ATProtocol? protocol;
    private Session? session;

    public ATConnection()
    {
    }

    public bool Connected => session != null;
    public SocialNetwork Network => SocialNetwork.AT;

    private static async Task RateLimit()
    {
        var now = DateTime.Now;
        var elapsed = now - lastAction;
        if (elapsed.TotalMilliseconds < RATE_ms)
        {
            var delay = RATE_ms - elapsed.TotalMilliseconds;
            await Task.Delay((int)delay);
        }
        lastAction = now;
    }

    public async Task<bool> ConnectAsync(INetworkCredentials? credentials)
    {
        if (credentials == null) throw new NullReferenceException($"Credentials are required.");
        var (valid, errors) = credentials.Validate();
        if (!valid) { throw new ArgumentException(string.Join(", ", errors)); }

        this.Credentials = credentials;
        this.protocol = new ATProtocolBuilder()
            .EnableAutoRenewSession(true)
            .WithInstanceUrl(Server)
            .Build();

        await RateLimit();
        var (session, error) = await protocol.AuthenticateWithPasswordResultAsync(account!, password!);
        if (error != null)
        {
            throw new Exception($"{error.StatusCode}: {error.Detail}");
        }
        this.session = session;
        return Connected;
    }

    public void Disconnect()
    {
        session = null;
        protocol?.Dispose();
        protocol = null;
    }

    public void Dispose()
    {
        Disconnect();
    }

    [MemberNotNull(nameof(protocol))]
    [MemberNotNull(nameof(session))]
    private void RequireAuthenticated()
    {
        if (protocol == null) throw new NullReferenceException("protocol is null");
        if (session == null) throw new NullReferenceException("session is null");
    }

    public async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null)
    {
        var atPost = new Post
        {
            Text = post.ComposeText(),
            Facets = new List<Facet>()
        };
        var response = await AtPostAsync(atPost, replyTo?.ReferenceKey);
        if (response == null) throw new NullReferenceException("Post reference not returned");
        return new ATPostReference(response.Uri!, post);
    }

    private async Task<CreateRecordOutput> AtPostAsync(Post post, string? replyKey = null)
    {
        RequireAuthenticated();
        await RateLimit();
        var result = await protocol.Feed.CreatePostAsync(post, rkey: replyKey, validate: true);
        return result.HandleResult()!;
    }

    private async Task<DeleteRecordOutput> DeletePostAsync(ATUri uri)
    {
        RequireAuthenticated();
        await RateLimit();
        var result = await protocol.Feed.DeletePostAsync(uri.Rkey);
        return result.HandleResult()!;
    }

}