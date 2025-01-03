using Presence.Posting.Lib.Connections.Test;

namespace Presence.Posting.Tests;

[TestClass]
[TestCategory("Unit")]
public class TestConnectionTests
{
    [TestMethod]
    public async Task TestConnection_Connects_WithNullCredentials()
    {
        var connection = new TestConnection();
        var ok = await connection.ConnectAsync(null);
        Assert.IsTrue(ok);
    }
}