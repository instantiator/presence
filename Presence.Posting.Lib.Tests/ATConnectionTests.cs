using System.Text;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using Presence.Posting.Lib.Config;
using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Connections.AT;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Networks.AT;
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
        var account = new ATAccount("TEST1", new EnvironmentConfigReader(env)["TEST1"][SocialNetwork.AT]);
        var (ok, errors) = account.Validate();
        Assert.IsTrue(ok, string.Join(", ", errors));
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ConnectionFactory_Creates_ATConnection()
    {
        var env = Environment.GetEnvironmentVariables();
        var environment = new EnvironmentConfigReader(env)["TEST1"][SocialNetwork.AT];
        var connection = ConnectionFactory.CreateConnection("TEST1", SocialNetwork.AT, environment);
        Assert.IsNotNull(connection);
        await connection.ConnectAsync();
        Assert.IsTrue(connection.Connected);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ATConnection_Posts_Post()
    {
        var env = Environment.GetEnvironmentVariables();
        var environment = new EnvironmentConfigReader(env)["TEST1"][SocialNetwork.AT];
        var connection = ConnectionFactory.CreateConnection("TEST1", SocialNetwork.AT, environment);
        await connection.ConnectAsync();
        var post = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES)
        {
            Message = [new SocialSnippet($"ATConnection_Posts_Post: {DateTime.Now:O}")]
        };
        var result = await connection.PostAsync(post);
        Assert.IsTrue(result.Network == SocialNetwork.AT);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.NetworkReferences["rkey"]));
        var resultAsAT = (ATPostReference)result;
        Assert.AreEqual(post, resultAsAT.Origin);
        Assert.IsNotNull(resultAsAT.Uri);
        Assert.IsFalse(string.IsNullOrWhiteSpace(resultAsAT.Uri.Rkey));
        Assert.IsFalse(string.IsNullOrWhiteSpace(resultAsAT.Uri.Did?.ToString()));
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ATConnection_Posts_Replies()
    {
        var env = Environment.GetEnvironmentVariables();
        var environment = new EnvironmentConfigReader(env)["TEST1"][SocialNetwork.AT];
        var connection = ConnectionFactory.CreateConnection("TEST1", SocialNetwork.AT, environment);
        await connection.ConnectAsync();
        var post0 = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES) { Message = [new SocialSnippet($"ATConnection_Posts_Replies (part 1): {DateTime.Now:O}")] };
        var post1 = new CommonPost(1, ATThreadComposer.AT_POST_RENDER_RULES) { Message = [new SocialSnippet($"ATConnection_Posts_Replies (part 2): {DateTime.Now:O}")] };

        var ref0 = await connection.PostAsync(post0);
        Assert.IsNotNull(ref0);
        Assert.IsFalse(string.IsNullOrWhiteSpace(ref0.NetworkReferences["rkey"]));

        var ref1 = await connection.PostAsync(post1, ref0);
        Assert.IsNotNull(ref1);
        Assert.IsFalse(string.IsNullOrWhiteSpace(ref1.NetworkReferences["rkey"]));
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ATConnection_Posts_Thread()
    {
        var env = Environment.GetEnvironmentVariables();
        var environment = new EnvironmentConfigReader(env)["TEST1"][SocialNetwork.AT];
        var connection = ConnectionFactory.CreateConnection("TEST1", SocialNetwork.AT, environment);
        await connection.ConnectAsync();
        var thread = new[]
        {
            new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES) { Message = [new SocialSnippet($"ATConnection_Posts_Thread (part 1): {DateTime.Now:O}")] },
            new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES) { Message = [new SocialSnippet($"ATConnection_Posts_Thread (part 2): {DateTime.Now:O}")] },
        };
        var result = await connection.PostAsync(thread);
        Assert.AreEqual(thread.Length, result.Count());
        Assert.IsTrue(result.All(r => r.Network == SocialNetwork.AT));
        Assert.IsTrue(result.All(r => r.Origin != null));
        Assert.IsTrue(result.All(r => r is ATPostReference));
        Assert.IsTrue(result.All(r => ((ATPostReference)r).Output != null));
        Assert.IsTrue(result.All(r => ((ATPostReference)r).Cid != null));
        Assert.IsTrue(result.All(r => ((ATPostReference)r).Did != null));
        Assert.IsTrue(result.All(r => ((ATPostReference)r).Uri != null));
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ATConnection_Posts_WithLinkAndTagFacets()
    {
        var env = Environment.GetEnvironmentVariables();
        var environment = new EnvironmentConfigReader(env)["TEST1"][SocialNetwork.AT];
        var connection = ConnectionFactory.CreateConnection("TEST1", SocialNetwork.AT, environment);
        await connection.ConnectAsync();
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
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.NetworkReferences["rkey"]));
    }

    [TestMethod]
    [TestCategory("Unit")]
    public async Task ATConnection_IdentifiesFacets()
    {
        var connection = new ATConnection(new ATAccount("TEST1", new Dictionary<NetworkCredentialType, string?>()));
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
        var environment = new EnvironmentConfigReader(env)["TEST1"][SocialNetwork.AT];
        var connection = ConnectionFactory.CreateConnection("TEST1", SocialNetwork.AT, environment) as ATConnection;
        Assert.IsNotNull(connection);
        await connection.ConnectAsync();
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
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Uri?.Rkey));
    }
}