using System.Text;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Connections.AT;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Tests;

[TestClass]
public class ATConnectionTests
{
    [TestMethod]
    [TestCategory("Integration")]
    public void Environment_Contains_ATConnectionConfig()
    {
        var env = Environment.GetEnvironmentVariables();
        var credentials = new ATCredentials(env);
        var (ok, errors) = credentials.Validate();
        Assert.IsTrue(ok, string.Join(", ", errors));
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ConnectionFactory_Connects_ATConnection()
    {
        var env = Environment.GetEnvironmentVariables();
        var connection = await ConnectionFactory.CreateConnection(SocialNetwork.AT, env);
        Assert.IsNotNull(connection);
        Assert.IsTrue(connection.Connected);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ATConnection_Posts()
    {
        var env = Environment.GetEnvironmentVariables();
        var connection = await ConnectionFactory.CreateConnection(SocialNetwork.AT, env);
        var post = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES)
        {
            Message = [new SocialSnippet($"ATConnection_Posts: {DateTime.Now:O}")]
        };
        var result = await connection.PostAsync(post);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.ReferenceKey));
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ATConnection_Posts_WithLinkAndTagFacets()
    {
        var env = Environment.GetEnvironmentVariables();
        var connection = await ConnectionFactory.CreateConnection(SocialNetwork.AT, env);
        var post = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES)
        {
            Message =
            [
                new SocialSnippet($"ATConnection_Posts_WithLinkAndTagFacets: {DateTime.Now:O}"),
                new SocialSnippet($"instantiator", SnippetType.Link, "https://instantiator.dev"),
                new SocialSnippet($"test", SnippetType.Tag),
            ]
        };
        var result = await connection.PostAsync(post);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.ReferenceKey));
    }

    [TestMethod]
    [TestCategory("Unit")]
    public async Task ATConnection_IdentifiesFacets()
    {
        var connection = new ATConnection();
        var post = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES)
        {
            Message =
            [
                new SocialSnippet($"ATConnection_Posts_WithLinkAndTagFacets: {DateTime.Now:O}"),
                new SocialSnippet($"instantiator", SnippetType.Link, "https://instantiator.dev"),
                new SocialSnippet($"test", SnippetType.Tag),
            ],
        };
        var facets = await connection.GetFacetsAsync(post);
        Assert.AreEqual(2, facets.Count());

        var linkFacet = facets.ElementAt(0);
        Assert.IsNotNull(linkFacet.Index);
        var prefix1 = $"ATConnection_Posts_WithLinkAndTagFacets: {DateTime.Now:O} ";
        Assert.AreEqual(Encoding.Default.GetBytes(prefix1).Length, linkFacet.Index.ByteStart);
        Assert.AreEqual(Encoding.Default.GetBytes(prefix1 + "instantiator").Length, linkFacet.Index.ByteEnd);
        Assert.IsNotNull(linkFacet.Features);
        Assert.AreEqual(1, linkFacet.Features.Count());
        Assert.AreEqual("app.bsky.richtext.facet#link", linkFacet.Features[0].Type);

        var tagFacet = facets.ElementAt(1);
        Assert.IsNotNull(tagFacet.Index);
        var prefix2 = $"ATConnection_Posts_WithLinkAndTagFacets: {DateTime.Now:O} instantiator ";
        Assert.AreEqual(Encoding.Default.GetBytes(prefix2).Length, tagFacet.Index.ByteStart);
        Assert.AreEqual(Encoding.Default.GetBytes(prefix2 + "#test").Length, tagFacet.Index.ByteEnd);
        Assert.IsNotNull(tagFacet.Features);
        Assert.AreEqual(1, tagFacet.Features.Count());
        Assert.AreEqual("app.bsky.richtext.facet#tag", tagFacet.Features[0].Type);
    }

    [TestMethod]
    [TestCategory("Integration")]
    [DataRow("https://instantiator.dev/presence/images/icon.png", "Presence icon (url)")]
    [DataRow("file:///SampleData/icon.png", "Presence icon (file)")]
    [DataRow("file:/SampleData/icon.png", "Presence icon (file)")]
    public async Task ATConnection_UploadsImage(string uri, string alt)
    {
        var env = Environment.GetEnvironmentVariables();
        var connection = (ATConnection)await ConnectionFactory.CreateConnection(SocialNetwork.AT, env);
        var image = new CommonPostImage
        {
            SourceUrl = uri,
            AltText = alt,
        };

        var post = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES)
        {
            Message =
            [
                new SocialSnippet($"ATConnection_UploadsImage: {DateTime.Now:O}"),
                new SocialSnippet($"From path: {uri}"),
                new SocialSnippet("Icon courtesy of:"),
                new SocialSnippet("icons8.com", SnippetType.Link, "https://icons8.com"),
            ],
            Images = [image],
        };
        var images = await Task.WhenAll(post.Images.Select(async i => await connection.UploadImage(i)));
        Assert.IsTrue(image.Uploaded);
        Assert.IsNotNull(images);
        Assert.AreEqual(post.Images.Count(), images.Length);

        var embed = new EmbedImages(images: images.ToList());
        var atPost = new Post
        {
            Text = post.ComposeText(),
            Facets = await connection.GetFacetsAsync(post),
            Embed = embed,
        };

        var response = await connection.AtPostAsync(atPost);
        var reference = new ATPostReference(response.Uri!, post);
        Assert.IsFalse(string.IsNullOrWhiteSpace(reference.ReferenceKey));
    }
}