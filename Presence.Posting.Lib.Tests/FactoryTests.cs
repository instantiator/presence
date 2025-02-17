using Presence.Posting.Lib.Config;
using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class FactoryTests
{
    private static readonly Dictionary<string,string> config = new()
    {
        ["PRESENCE_ACCOUNTS"]="TEST0,TEST1,TEST2",
        ["TEST0_Console_PrintPrefix"]="Console>",
        ["TEST1_AT_AccountName"]="presence-lib-test.bsky.social",
        ["TEST1_AT_AppPassword"]="app-password-goes-here",
        ["TEST2_SlackWebhook_IncomingWebhookUrl"]="https://hooks.slack.com/services/XXXXXXXXX/XXXXXXXXX/XXXXXXXXXXXXXXXXXXXXXXXX"
    };

    [TestMethod]
    [DataRow("TEST0", SocialNetwork.Console)]
    [DataRow("TEST1", SocialNetwork.AT)]
    [DataRow("TEST2", SocialNetwork.SlackWebhook)]
    public void ConnectionFactory_CanCreate_Connection(string prefix, SocialNetwork network)
    {
        var env = new EnvironmentConfigReader(config);
        var connection = ConnectionFactory.CreateConnection(prefix, network, env[prefix][network]);
        Assert.IsNotNull(connection, $"ConnectionFactory could not create an INetworkConnection for: {network}");
        Assert.IsTrue(connection.Network == network);
        Assert.IsFalse(connection.Connected);
    }
}