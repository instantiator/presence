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
using Presence.SocialFormat.Lib.Helpers;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.AT;

public class ATConnection : AbstractNetworkConnection
{
    private const int RATE_ms = 1000;
    private static DateTime lastAction = DateTime.MinValue;

    public Uri Server(INetworkCredentials? credentials)
        => credentials?.ContainsKey(NetworkCredentialType.Server) == true && !string.IsNullOrWhiteSpace(credentials[NetworkCredentialType.Server])
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
        return new ATPostReference(response, post);
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
        var blobResult = await protocol!.Repo.UploadBlobAsync(content);
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

    public async Task<CreateRecordOutput> AtPostAsync(Post post, ATPostReference? replyTo = null)
    {
        RequireAuthenticated();
        await RateLimitAsync();

        var reply = replyTo != null
            ? new ReplyRefDef(new StrongRef(replyTo.Uri, replyTo.Cid), new StrongRef(replyTo.Uri, replyTo.Cid))
            : null;

        var result = await protocol.Feed.CreatePostAsync(post.Text, post.Facets, reply: reply, validate: true);
        return result.HandleResult()!;
    }

    public override async Task<bool> DeletePostAsync(INetworkPostReference reference)
    {
        RequireAuthenticated();
        await RateLimitAsync();
        var result = await protocol.Feed.DeletePostAsync(reference.NetworkReferences["rkey"]);
        var output = result.HandleResult()!;
        return output != null;
    }

}