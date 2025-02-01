using Presence.Posting.Lib.Connections;

namespace Presence.Posting.Tests;

[TestClass]
[TestCategory("Unit")]
public class CredentialTests
{
    [TestMethod]
    public void ATCredentials_Validate()
    {
        var creds = new ATCredentials();
        var (valid, errors) = creds.Validate();
        Assert.IsFalse(valid);
        Assert.AreEqual(2, errors.Count());
        Assert.IsTrue(errors.Contains("AccountName is required"));
        Assert.IsTrue(errors.Contains("AppPassword is required"));

        creds[NetworkCredentialType.AccountName] = "test";
        (valid, errors) = creds.Validate();
        Assert.IsFalse(valid);
        Assert.AreEqual(1, errors.Count());
        Assert.IsTrue(errors.Contains("AppPassword is required"));

        creds[NetworkCredentialType.AppPassword] = "test";
        (valid, errors) = creds.Validate();
        Assert.IsTrue(valid);
        Assert.AreEqual(0, errors.Count());
    }

    [TestMethod]
    public void ATCredentials_AcceptEnvVariables()
    {
        var env = new Dictionary<string, string>
        {
            { "AT_AccountName", "test" },
            { "AT_AppPassword", "test" }
        };
        var creds = new ATCredentials(env);
        var (valid, errors) = creds.Validate();
        Assert.IsTrue(valid);
        Assert.AreEqual(0, errors.Count());
    }

}