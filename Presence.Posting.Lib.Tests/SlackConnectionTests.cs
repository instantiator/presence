using Presence.Posting.Lib.Config;
using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Connections.Slack;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Networks.Slack;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Tests;

[TestClass]
public class SlackConnectionTests
{
    [TestMethod]
    [TestCategory("Integration")]
    public void Environment_Contains_SlackWebhookConnectionConfig()
    {
        var env = Environment.GetEnvironmentVariables();
        var account = new SlackWebhookAccount("TEST2", new EnvironmentConfigReader(env)["TEST2"][SocialNetwork.SlackWebhook]);
        var (ok, errors) = account.Validate();
        Assert.IsTrue(ok, string.Join(", ", errors));
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task ConnectionFactory_Creates_SlackWebhookConnection()
    {
        var env = Environment.GetEnvironmentVariables();
        var environment = new EnvironmentConfigReader(env)["TEST2"][SocialNetwork.SlackWebhook];
        var connection = ConnectionFactory.CreateConnection("TEST2", SocialNetwork.SlackWebhook, environment);
        Assert.IsNotNull(connection);
        await connection.ConnectAsync();
        Assert.IsTrue(connection.Connected);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task SlackWebhookConnection_Posts_Post()
    {
        var env = Environment.GetEnvironmentVariables();
        var environment = new EnvironmentConfigReader(env)["TEST2"][SocialNetwork.SlackWebhook];
        var connection = ConnectionFactory.CreateConnection("TEST2", SocialNetwork.SlackWebhook, environment);
        await connection.ConnectAsync();
        var post = new CommonPost(0, SlackThreadComposer.SLACK_POST_RENDER_RULES)
        {
            Message = [new SocialSnippet($"SlackWebhookConnection_Posts_Post: {DateTime.Now:O}")]
        };
        var result = await connection.PostAsync(post);
        Assert.IsTrue(result.Network == SocialNetwork.SlackWebhook);
        var resultAsSlack = (SlackWebhookPostReference)result;
        Assert.AreEqual(post, resultAsSlack.Origin);
    }
}