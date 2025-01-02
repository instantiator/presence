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

namespace Presence.Posting.Lib.Connections.BlueSky;

public class ATConnection : INetworkConnection
{
    private const int RATE_ms = 1000;
    private static DateTime lastAction = DateTime.MinValue;

    public string Server { get; private set; }
    public string Account { get; private set; }
    private string password;

    private ATProtocol protocol;
    private Session? session;

    public ATConnection(string server, string account, string password)
    {
        this.Server = server;
        this.Account = account;
        this.password = password;

        this.protocol = new ATProtocolBuilder()
            .EnableAutoRenewSession(true)
            .WithInstanceUrl(new Uri("https://" + server))
            .Build();
    }

    public bool Authenticated => session != null;

    public SocialNetwork Network => throw new NotImplementedException();

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

    public async Task<bool> ConnectAsync()
    {
        await RateLimit();
        var (session, error) = await protocol.AuthenticateWithPasswordResultAsync(Account, password);
        if (error != null)
        {
            throw new Exception($"{error.StatusCode}: {error.Detail}");
        }
        this.session = session;
        return Authenticated;
    }

    public void Disconnect()
    {
        session = null;
    }

    public void Dispose()
    {
        Disconnect();
    }

    [MemberNotNull(nameof(session))]
    private void RequireAuthenticated()
    {
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