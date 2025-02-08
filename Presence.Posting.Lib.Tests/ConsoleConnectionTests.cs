using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Connections.Console;
using Presence.Posting.Lib.Constants;

namespace Presence.Posting.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class ConsoleConnectionTests
{
    [TestMethod]
    public async Task ConsoleConnection_Connects_WithSingleCredential()
    {
        var connection = new ConsoleConnection(new ConsoleAccount("TEST", new Dictionary<NetworkCredentialType, string?>() { { NetworkCredentialType.PrintPrefix, "TEST> " } }));
        var ok = await connection.ConnectAsync();
        Assert.IsTrue(ok);
    }

    [TestMethod]
    public async Task ConnectionFactory_Creates_ConsoleConnection()
    {
        var connection = ConnectionFactory.CreateConsole("TEST", new Dictionary<NetworkCredentialType, string?>() { { NetworkCredentialType.PrintPrefix, "TEST> " } });
        Assert.IsNotNull(connection);
        await connection.ConnectAsync();
        Assert.IsTrue(connection.Connected);
    }

}