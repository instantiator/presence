using System.Collections;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public class ATCredentials : AbstractNetworkCredentials
{
    public ATCredentials() : base(SocialNetwork.AT)
    {
    }

    public ATCredentials(IDictionary<NetworkCredentialType, string> credentials) : base(SocialNetwork.AT, credentials)
    {
    }

    public ATCredentials(IDictionary? env) : base(env)
    {
    }

    public override string Prefix => "AT_";

    public override (bool, IEnumerable<string>) Validate()
    {
        var accountOk = ContainsKey(NetworkCredentialType.AccountName) && !string.IsNullOrWhiteSpace(this[NetworkCredentialType.AccountName]);
        var passwordOk = ContainsKey(NetworkCredentialType.AppPassword) && !string.IsNullOrWhiteSpace(this[NetworkCredentialType.AppPassword]);
        var errors = new List<string>();
        if (!accountOk) { errors.Add("AccountName is required"); }
        if (!passwordOk) { errors.Add("AppPassword is required"); }
        return (accountOk && passwordOk, errors);
    }
}