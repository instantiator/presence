using System.Text;
using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Connections.AT;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Tests;

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
            ]
        };
        var facets = await connection.GetFacetsAsync(post);

        Assert.AreEqual(1, facets.Count());
        var linkFacet = facets.ElementAt(0);
        Assert.IsNotNull(linkFacet.Index);
        var prefix = $"ATConnection_Posts_WithLinkAndTagFacets: {DateTime.Now:O} ";
        Assert.AreEqual(Encoding.Default.GetBytes(prefix).Length, linkFacet.Index.ByteStart);
        Assert.AreEqual(Encoding.Default.GetBytes(prefix + "instantiator").Length, linkFacet.Index.ByteEnd);
        Assert.IsNotNull(linkFacet.Features);
        Assert.AreEqual(1, linkFacet.Features.Count());
        Assert.AreEqual("app.bsky.richtext.facet#link", linkFacet.Features[0].Type);
    }
}