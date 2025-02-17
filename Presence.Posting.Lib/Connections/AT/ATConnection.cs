using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
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

    public Uri Server(INetworkAccount? account)
        => account?.ContainsKey(NetworkCredentialType.Server) == true && !string.IsNullOrWhiteSpace(account[NetworkCredentialType.Server])
            ? new Uri("https://" + account[NetworkCredentialType.Server])
            : new Uri("https://bsky.social");

    public ATProtocol? Protocol;
    public Session? Session;
    public ATHandle? Author;

    [SetsRequiredMembers]
    public ATConnection(ATAccount account) : base(account)
    {
    }

    public override bool Connected => Session != null;

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

    protected override async Task<bool> ConnectImplementationAsync(INetworkAccount credentials)
    {
        this.Protocol = new ATProtocolBuilder()
            .EnableAutoRenewSession(true)
            .WithInstanceUrl(Server(credentials))
            .Build();

        await RateLimitAsync();
        var (session, error) = await Protocol.AuthenticateWithPasswordResultAsync(
            credentials![NetworkCredentialType.AccountName]!,
            credentials![NetworkCredentialType.AppPassword]!);

        if (error != null)
        {
            throw new Exception($"{error.StatusCode}: {error.Detail}");
        }
        this.Session = session;
        this.Author = session!.Handle;
        
        return Connected;
    }

    protected override async Task DisconnectImplementationAsync()
    {
        Session = null;
        Protocol?.Dispose();
        Protocol = null;
    }

    [MemberNotNull(nameof(Protocol))]
    [MemberNotNull(nameof(Session))]
    private void RequireAuthenticated()
    {
        if (Protocol == null) throw new NullReferenceException("protocol is null");
        if (Session == null) throw new NullReferenceException("session is null");
    }

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = default)
    {
        var images = await Task.WhenAll(post.Images.Select(async i => await UploadImage(i)));
        var embed = images.Any() ? new EmbedImages(images: images.ToList()) : null;
        var atPost = new Post
        {
            Text = post.ComposeText(),
            Facets = await GetFacetsAsync(post),
            Embed = embed,
        };

        var response = await AtPostAsync(atPost, replyTo as ATPostReference);
        if (response == null) throw new NullReferenceException("Post reference not returned");
        if (response.Uri == null) throw new NullReferenceException("Post reference did not contain a Uri");
        if (string.IsNullOrWhiteSpace(response.Cid)) throw new NullReferenceException("Post reference did not contain a Cid");
        return new ATPostReference(response, Server(Account).Host, Author!.Handle, post);
    }

    public async Task<Image> UploadImage(CommonPostImage cpi)
    {
        var uri = cpi.SourceUrl.ToUri();
        if (uri == null) { throw new Exception($"Could not parse image uri: {cpi.SourceUrl}"); }
        var image = await UploadImage(uri!, cpi.AltText);
        if (image == null) { throw new Exception($"Could not upload image: {cpi.SourceUrl}"); }
        cpi.Uploaded = true;
        return image;
    }

    public async Task<Image> UploadImage(Uri src, string alt)
    {
        RequireAuthenticated();
        var metadata = src.GetMetadata();
        if (!metadata.IsImage) { throw new Exception($"Uri target is not an image: {src}"); }
        if (!metadata.IsFile && !metadata.IsHttp) { throw new Exception($"Uri is neither file or link {src}"); }
        if (!metadata.Exists) { throw new Exception($"Image not found: {src}"); }

        using var httpClient = new HttpClient();
        var stream = await src.GetStreamAsync(metadata);
        var content = new StreamContent(stream);
        content.Headers.ContentLength = stream.CanSeek ? stream.Length : null;
        content.Headers.ContentType = new MediaTypeHeaderValue(metadata.MimeType!);

        await RateLimitAsync();
        var blobResult = await Protocol!.Repo.UploadBlobAsync(content);
        var success = blobResult.HandleResult();
        if (success != null)
        {
            return new Image(success.Blob, alt);
        }
        else
        {
            throw new Exception($"Error uploading image");
        }
    }

    public async Task<List<Facet>> GetFacetsAsync(CommonPost post)
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
                    facets.Add(Facet.CreateFacetLink(position, position + length, reference!));
                    break;
                case SnippetType.Tag:
                    text.RequireText("Tag text");
                    // The tag .Text does not contain the # prefix, which is correct for the Facet.CreateFacetHashtag.
                    // The strings found in allData come from CommonPost.Compose, which does contain the prefix, which is correct.
                    facets.Add(Facet.CreateFacetHashtag(position, position + length, text!));
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

        return result.HandleResult()!;
    }

    public override async Task<bool> DeletePostAsync(INetworkPostReference reference)
    {
        RequireAuthenticated();
        await RateLimitAsync();
        var result = await Protocol.Feed.DeletePostAsync(((ATPostReference)reference.NetworkReferences).RKey);
        var output = result.HandleResult()!;
        return output != null;
    }

}