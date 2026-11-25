using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using FishyFlip;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Richtext;
using FishyFlip.Lexicon.Com.Atproto.Repo;
using FishyFlip.Models;
using FishyFlip.Tools;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Helpers;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.AT;

public class ATConnection : AbstractNetworkConnection
{
    private const int RATE_ms = 1000;

    public static Uri Server(INetworkAccount? account)
        => account?.ContainsKey(NetworkCredentialType.Server) == true && !string.IsNullOrWhiteSpace(account[NetworkCredentialType.Server])
            ? new Uri("https://" + account[NetworkCredentialType.Server])
            : new Uri("https://bsky.social");

    public ATProtocol? Protocol { get; private set; }
    public Session? Session { get; private set; }
    public ATHandle? Author { get; private set; }

    [SetsRequiredMembers]
    public ATConnection(ATAccount account) : base(account)
    {
    }

    public override bool Connected => Session != null;

    protected override async Task RateLimitImplementationAsync(DateTime? lastAction)
    {
        if (lastAction == null) { return; }
        var now = DateTime.Now;
        var elapsed = now - lastAction.Value;
        if (elapsed.TotalMilliseconds < RATE_ms)
        {
            var delay = RATE_ms - elapsed.TotalMilliseconds;
            await Task.Delay((int)delay);
        }
    }

    protected override async Task<bool> ConnectImplementationAsync(INetworkAccount account)
    {
        this.Protocol = new ATProtocolBuilder()
            .EnableAutoRenewSession(true)
            .WithInstanceUrl(Server(account))
            .Build();

        await RateLimitAsync();
        var (session, error) = await Protocol.AuthenticateWithPasswordResultAsync(
            RequireCredential(account, NetworkCredentialType.AccountName),
            RequireCredential(account, NetworkCredentialType.AppPassword));

        if (error != null)
        {
            throw new AuthenticationException($"{error.StatusCode}: {error.Detail}");
        }
        this.Session = session ?? throw new AuthenticationException("Authentication returned no session");
        this.Author = session.Handle;

        return Connected;
    }

    protected override async Task DisconnectImplementationAsync()
    {
        Session = null;
        Protocol?.Dispose();
        Protocol = null;
    }

    private static string RequireCredential(INetworkAccount account, NetworkCredentialType type)
        => account.TryGetValue(type, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException($"Missing credential: {type}", nameof(account));

    [MemberNotNull(nameof(Protocol), nameof(Session), nameof(Author))]
    private void RequireAuthenticated()
    {
        if (Protocol == null || Session == null || Author == null)
        {
            throw new InvalidOperationException("Not connected: call ConnectAsync first");
        }
    }

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = default)
    {
        RequireAuthenticated();
        var images = await Task.WhenAll(post.Images.Select(async i => await UploadImage(i)));
        var embed = images.Length > 0 ? new EmbedImages(images: images.ToList()) : null;
        var atPost = new Post
        {
            Text = post.ComposeText(),
            Facets = GetFacets(post),
            Embed = embed,
        };

        var response = await AtPostAsync(atPost, replyTo as ATPostReference);
        return new ATPostReference(response, Server(Account).Host, Author.Handle, post);
    }

    public async Task<Image> UploadImage(CommonPostImage cpi)
    {
        var uri = cpi.SourceUrl.ToUri();
        if (uri == null) { throw new ArgumentException($"Could not parse image uri: {cpi.SourceUrl}", nameof(cpi)); }
        var image = await UploadImage(uri, cpi.AltText);
        cpi.Uploaded = true;
        return image;
    }

    public async Task<Image> UploadImage(Uri src, string alt)
    {
        RequireAuthenticated();
        var metadata = src.GetMetadata();
        if (!metadata.IsImage) { throw new ArgumentException($"Uri target is not an image: {src}", nameof(src)); }
        if (!metadata.IsFile && !metadata.IsHttp) { throw new ArgumentException($"Uri is neither file or link {src}", nameof(src)); }
        if (!metadata.Exists) { throw new ArgumentException($"Image not found: {src}", nameof(src)); }
        var mimeType = metadata.MimeType ?? throw new ArgumentException($"Could not determine the image type: {src}", nameof(src));

        using var httpClient = new HttpClient();
        var stream = await src.GetStreamAsync(metadata, httpClient);
        var content = new StreamContent(stream);
        content.Headers.ContentLength = stream.CanSeek ? stream.Length : null;
        content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

        await RateLimitAsync();
        var blobResult = await Protocol.Repo.UploadBlobAsync(content);
        var success = blobResult.HandleResult();
        if (success != null)
        {
            return new Image(success.Blob, alt);
        }
        else
        {
            throw new InvalidOperationException($"Error uploading image: {src}");
        }
    }

    public static List<Facet> GetFacets(CommonPost post)
    {
        var facets = new List<Facet>();
        var allItems = post.Compose().Select((pair, index) => new { pair, index });
        foreach (var item in allItems)
        {
            var type = item.pair.Item1?.SnippetType;
            var reference = item.pair.Item1?.Reference;
            var text = item.pair.Item1?.Text;
            var position = Encoding.Default.GetBytes(string.Join(string.Empty, allItems.Take(item.index).Select(d => d.pair.Item2))).Length;
            var length = Encoding.Default.GetBytes(item.pair.Item2).Length;

            switch (type)
            {
                case SnippetType.Link:
                    reference.RequireText("Link reference");
                    facets.Add(Facet.CreateFacetLink(position, position + length, reference));
                    break;
                case SnippetType.Tag:
                    text.RequireText("Tag text");
                    // The tag .Text does not contain the # prefix, which is correct for the Facet.CreateFacetHashtag.
                    // The strings found in allData come from CommonPost.Compose, which does contain the prefix, which is correct.
                    facets.Add(Facet.CreateFacetHashtag(position, position + length, text));
                    break;
            }
        }
        return facets;
    }

    public async Task<CreateRecordOutput> AtPostAsync(Post post, ATPostReference? replyTo = default)
    {
        RequireAuthenticated();
        await RateLimitAsync();

        var reply = replyTo != null
            ? new ReplyRefDef(new StrongRef(replyTo.Uri, replyTo.Cid), new StrongRef(replyTo.Uri, replyTo.Cid))
            : null;

        var result = await Protocol.Feed.CreatePostAsync(
            text: post.Text,
            facets: post.Facets,
            embed: post.Embed,
            reply: reply,
            validate: true);

        return result.HandleResult() ?? throw new InvalidOperationException("Post was not created");
    }

    public override async Task<bool> DeletePostAsync(INetworkPostReference reference)
    {
        RequireAuthenticated();
        await RateLimitAsync();
        if (reference is not ATPostReference atReference)
        {
            throw new ArgumentException($"Not an AT post reference: {reference.Network}", nameof(reference));
        }
        var result = await Protocol.Feed.DeletePostAsync(atReference.RKey);
        return result.HandleResult() != null;
    }

}