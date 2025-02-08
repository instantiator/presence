using System.Collections;
using System.Text.Json;
using Presence.Posting.Lib.Config;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Tests;

[TestClass]
public class EnvironmentConfigReaderTests
{

    [TestMethod]
    [TestCategory("Unit")]
    public void EnvironmentConfigReader_ExtractsIgnoringCase_MultipleAccounts()
    {
        var environments = new List<IDictionary>()
        {
            new Dictionary<string,string>()
            {
                { ConfigKeys.ACCOUNTS_ENV_KEY, "TEST0,TEST1" },
                { "TEST0_CONSOLE_PRINTPREFIX", "printprefix:test0" },
                { "TEST1_AT_ACCOUNTNAME", "account:test1" },
                { "TEST1_AT_APPPASSWORD", "password:test1" }
            },
            new Dictionary<string,string>()
            {
                { ConfigKeys.ACCOUNTS_ENV_KEY, "TEST0,TEST1" },
                { "TEST0_console_printprefix", "printprefix:test0" },
                { "TEST1_at_accountname", "account:test1" },
                { "TEST1_at_apppassword", "password:test1" }
            }
        };

        foreach (var env in environments)
        {
            var reader = new EnvironmentConfigReader(env);
            Assert.AreEqual(2, reader.Count);
            Assert.IsTrue(reader.ContainsKey("TEST0"));
            Assert.IsTrue(reader.ContainsKey("TEST1"));
            Assert.AreEqual("printprefix:test0", reader["TEST0"][SocialNetwork.Console][NetworkCredentialType.PrintPrefix]);
            Assert.AreEqual("account:test1", reader["TEST1"][SocialNetwork.AT][NetworkCredentialType.AccountName]);
            Assert.AreEqual("password:test1", reader["TEST1"][SocialNetwork.AT][NetworkCredentialType.AppPassword]);
        }
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void EnvironmentConfigReader_ExtractsOnly_AccountsInTheAccountsVariable()
    {
        var env = new Dictionary<string, string>()
        {
            { ConfigKeys.ACCOUNTS_ENV_KEY, "TEST0,TEST1" },
            { "TEST0_CONSOLE_PRINTPREFIX", "printprefix:test0" },
            { "TEST1_AT_ACCOUNTNAME", "account:test1" },
            { "TEST1_AT_APPPASSWORD", "password:test1" },
            { "TEST2_AT_ACCOUNTNAME", "account:test2" },
            { "TEST2_AT_APPPASSWORD", "password:test2" },
        };
        var reader = new EnvironmentConfigReader(env);
        Assert.AreEqual(2, reader.Count);
        Assert.IsTrue(reader.ContainsKey("TEST0"));
        Assert.IsTrue(reader.ContainsKey("TEST1"));
        Assert.IsFalse(reader.ContainsKey("TEST2"));
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void EnvironmentConfigReader_CanExtract_JsonConfiguration()
    {
        var env = new Dictionary<string, string>()
        {
            { ConfigKeys.JSON_DATA_ENV_KEY, @"
                {
                    ""PRESENCE_ACCOUNTS"": ""TEST0,TEST1"",
                    ""TEST0_CONSOLE_PRINTPREFIX"": ""printprefix:test0"",
                    ""TEST1_AT_ACCOUNTNAME"": ""account:test1"",
                    ""TEST1_AT_APPPASSWORD"": ""password:test1""
                }" }
        };
        var reader = new EnvironmentConfigReader(env);
        Assert.AreEqual(2, reader.Count, JsonSerializer.Serialize(reader));
        Assert.IsTrue(reader.ContainsKey("TEST0"));
        Assert.IsTrue(reader.ContainsKey("TEST1"));
        Assert.AreEqual("printprefix:test0", reader["TEST0"][SocialNetwork.Console][NetworkCredentialType.PrintPrefix]);
        Assert.AreEqual("account:test1", reader["TEST1"][SocialNetwork.AT][NetworkCredentialType.AccountName]);
        Assert.AreEqual("password:test1", reader["TEST1"][SocialNetwork.AT][NetworkCredentialType.AppPassword]);
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void EnvironmentConfigReader_JsonConfiguration_Supersedes_OtherKeys()
    {
        var env = new Dictionary<string, string>()
        {
            { 
                ConfigKeys.JSON_DATA_ENV_KEY, @"
                {
                    ""PRESENCE_ACCOUNTS"": ""TEST0,TEST1"",
                    ""TEST0_CONSOLE_PRINTPREFIX"": ""json:printprefix:test0"",
                    ""TEST1_AT_ACCOUNTNAME"": ""json:account:test1"",
                    ""TEST1_AT_APPPASSWORD"": ""json:password:test1""
                }" 
            },
            { "PRESENCE_ACCOUNTS", "TEST0" },
            { "TEST0_CONSOLE_PRINTPREFIX", "ignored:printprefix:test0" },
            { "TEST1_AT_ACCOUNTNAME", "ignored:account:test1" },
            { "TEST1_AT_APPPASSWORD", "ignored:password:test1" }
        };
        var reader = new EnvironmentConfigReader(env);
        Assert.AreEqual(2, reader.Count, JsonSerializer.Serialize(reader));
        Assert.IsTrue(reader.ContainsKey("TEST0"));
        Assert.IsTrue(reader.ContainsKey("TEST1"));
        Assert.AreEqual("json:printprefix:test0", reader["TEST0"][SocialNetwork.Console][NetworkCredentialType.PrintPrefix]);
        Assert.AreEqual("json:account:test1", reader["TEST1"][SocialNetwork.AT][NetworkCredentialType.AccountName]);
        Assert.AreEqual("json:password:test1", reader["TEST1"][SocialNetwork.AT][NetworkCredentialType.AppPassword]);
    }

}