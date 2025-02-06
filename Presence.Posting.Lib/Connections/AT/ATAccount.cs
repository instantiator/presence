using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public class ATAccount : AbstractNetworkAccount
{
    public ATAccount(string prefix, IDictionary<NetworkCredentialType, string> credentials) : base(prefix, credentials)
    {
    }

    public override IEnumerable<NetworkCredentialType> AcceptedCredentials => 
    [ 
        NetworkCredentialType.AccountName, 
        NetworkCredentialType.AppPassword, 
        NetworkCredentialType.Server 
    ];

    public override IEnumerable<NetworkCredentialType> RequiredCredentials => 
    [ 
        NetworkCredentialType.AccountName, 
        NetworkCredentialType.AppPassword
    ];

    public override SocialNetwork SocialNetwork => SocialNetwork.AT;
}