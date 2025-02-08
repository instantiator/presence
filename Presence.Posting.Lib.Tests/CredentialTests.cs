using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Constants;

namespace Presence.Posting.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class CredentialTests
{
    [TestMethod]
    public void ATAccount_Validates()
    {
        var account = new ATAccount("TEST", new Dictionary<NetworkCredentialType, string?>
        {
        });
        var (valid, errors) = account.Validate();
        Assert.IsFalse(valid);
        Assert.AreEqual(2, errors.Count());
        Assert.IsTrue(errors.Contains("Missing credential: AccountName"));
        Assert.IsTrue(errors.Contains("Missing credential: AppPassword"));

        account[NetworkCredentialType.AccountName] = "test";
        (valid, errors) = account.Validate();
        Assert.IsFalse(valid);
        Assert.AreEqual(1, errors.Count());
        Assert.IsTrue(errors.Contains("Missing credential: AppPassword"));

        account[NetworkCredentialType.AppPassword] = "test";
        (valid, errors) = account.Validate();
        Assert.IsTrue(valid);
        Assert.AreEqual(0, errors.Count());
    }
}