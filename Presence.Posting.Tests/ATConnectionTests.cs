using Presence.Posting.Lib.Connections;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Tests;

[TestClass]
[TestCategory("Integration")]
public class ATConnectionTests
{
    [TestMethod]
    public void Environment_Contains_ATConnectionConfig()
    {
        var env = Environment.GetEnvironmentVariables();
        var credentials = new ATCredentials(env);
        var (ok, errors) = credentials.Validate();
        Assert.IsTrue(ok, string.Join(", ", errors));
    }

    [TestMethod]
    public async Task ConnectionFactory_Connects_ATConnection()
    {
        var env = Environment.GetEnvironmentVariables();
        var connection = await ConnectionFactory.CreateConnection(SocialNetwork.AT, env);
        Assert.IsNotNull(connection);
        Assert.IsTrue(connection.Connected);
    }

    [TestMethod]
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
}