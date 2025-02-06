using Presence.Posting.Lib.Connections;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Tests;

[TestClass]
[TestCategory("Integration")]
public class ConfigTests
{
    [TestMethod]
    public void ConfigPresent()
    {
        var env = Environment.GetEnvironmentVariables();
        Assert.IsNotNull(env);

        var environment = new EnvironmentConfigReader(env);

        Assert.IsTrue(environment.ContainsKey("TEST0"));
        Assert.IsTrue(environment["TEST0"].ContainsKey(SocialNetwork.Console));
        Assert.AreEqual(1, environment["TEST0"][SocialNetwork.Console].Count());
        Assert.IsTrue(environment["TEST0"][SocialNetwork.Console].ContainsKey(NetworkCredentialType.PrintPrefix));

        Assert.IsTrue(environment.ContainsKey("TEST1"));
        Assert.IsTrue(environment["TEST1"].ContainsKey(SocialNetwork.AT));
        Assert.AreEqual(2, environment["TEST1"][SocialNetwork.AT].Count());
        Assert.IsTrue(environment["TEST1"][SocialNetwork.AT].ContainsKey(NetworkCredentialType.AccountName));
        Assert.IsTrue(environment["TEST1"][SocialNetwork.AT].ContainsKey(NetworkCredentialType.AppPassword));
    }

}