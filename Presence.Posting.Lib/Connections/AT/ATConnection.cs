using System.Diagnostics.CodeAnalysis;
using FishyFlip;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Richtext;
using FishyFlip.Lexicon.Com.Atproto.Repo;
using FishyFlip.Models;
using FishyFlip.Tools;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.AT;

public class ATConnection : AbstractNetworkConnection
{
    private const int RATE_ms = 1000;
    private static DateTime lastAction = DateTime.MinValue;

    public Uri Server(INetworkCredentials? credentials) 
        => credentials?.ContainsKey(NetworkCredentialType.Server) == true 
            ? new Uri("https://" + credentials[NetworkCredentialType.Server]) 
            : new Uri("https://bsky.social");

    private ATProtocol? protocol;
    private Session? session;

    public ATConnection()
    {
    }

    public override bool Connected => session != null;
    public override SocialNetwork Network => SocialNetwork.AT;

    protected override async Task RateLimitImplementationAsync(DateTime? lastAction)
    {
        if (lastAction == null) { return; }
        var now = DateTime.Now;
        var elapsed = (TimeSpan)(now - lastAction!);
        if (elapsed.TotalMilliseconds < RATE_ms)
        {
            var delay = RATE_ms - elapsed.TotalMilliseconds;
            await Task.Delay((int)delay);
        }
    }

    protected override async Task<bool> ConnectImplementationAsync(INetworkCredentials? credentials)
    {
        this.protocol = new ATProtocolBuilder()
            .EnableAutoRenewSession(true)
            .WithInstanceUrl(Server(credentials))
            .Build();

        await RateLimitAsync();
        var (session, error) = await protocol.AuthenticateWithPasswordResultAsync(
            credentials![NetworkCredentialType.AccountName], 
            credentials![NetworkCredentialType.AppPassword]);

        if (error != null)
        {
            throw new Exception($"{error.StatusCode}: {error.Detail}");
        }
        this.session = session;
        return Connected;
    }

    protected override async Task DisconnectImplementationAsync()
    {
        session = null;
        protocol?.Dispose();
        protocol = null;
    }

    [MemberNotNull(nameof(protocol))]
    [MemberNotNull(nameof(session))]
    private void RequireAuthenticated()
    {
        if (protocol == null) throw new NullReferenceException("protocol is null");
        if (session == null) throw new NullReferenceException("session is null");
    }

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null)
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
        await RateLimitAsync();
        var result = await protocol.Feed.CreatePostAsync(post, rkey: replyKey, validate: true);
        return result.HandleResult()!;
    }

    public override async Task<bool> DeletePostAsync(INetworkPostReference reference)
    {
        RequireAuthenticated();
        await RateLimitAsync();
        var result = await protocol.Feed.DeletePostAsync(reference.ReferenceKey);
        var output = result.HandleResult()!;
        return output != null;
    }

}