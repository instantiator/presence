using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib;

public class ConsoleAccount : AbstractNetworkAccount
{
    public ConsoleAccount(string accountPrefix, IDictionary<NetworkCredentialType, string?>? credentials = null) : base(accountPrefix, credentials)
    {
    }

    public override SocialNetwork SocialNetwork => SocialNetwork.Console;

    public override IEnumerable<NetworkCredentialType> AcceptedCredentials => [NetworkCredentialType.PrintPrefix];

    public override IEnumerable<NetworkCredentialType> RequiredCredentials => [NetworkCredentialType.PrintPrefix];
}