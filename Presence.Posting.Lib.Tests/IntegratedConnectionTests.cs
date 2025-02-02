using Presence.Posting.Lib.Connections;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class IntegratedConnectionTests
{

    [TestMethod]
    public async Task ConnectionFactory_Connects_TestConnection()
    {
        var connection = await ConnectionFactory.CreateConnection(SocialNetwork.Console, null);
        Assert.IsNotNull(connection);
        Assert.IsTrue(connection.Connected);
    }

}