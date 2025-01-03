using System.Collections;

namespace Presence.Posting.Lib.Connections;

public class ATCredentials : AbstractNetworkCredentials
{
    public ATCredentials() : base()
    {
    }

    public ATCredentials(IDictionary<NetworkCredentialType, string> credentials) : base(credentials)
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