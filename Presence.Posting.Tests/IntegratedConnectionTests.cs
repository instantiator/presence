using Presence.Posting.Lib.Connections;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Tests;

[TestClass]
[TestCategory("Unit")]
public class IntegratedConnectionTests
{

    [TestMethod]
    public async Task ConnectionFactory_Connects_TestConnection()
    {
        var connection = await ConnectionFactory.CreateConnection(SocialNetwork.Test, null);
        Assert.IsNotNull(connection);
        Assert.IsTrue(connection.Connected);
    }

}