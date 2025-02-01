using Presence.Posting.Lib.Connections.Console;

namespace Presence.Posting.Tests;

[TestClass]
[TestCategory("Unit")]
public class TestConnectionTests
{
    [TestMethod]
    public async Task TestConnection_Connects_WithNullCredentials()
    {
        var connection = new ConsoleConnection();
        var ok = await connection.ConnectAsync(null);
        Assert.IsTrue(ok);
    }
}