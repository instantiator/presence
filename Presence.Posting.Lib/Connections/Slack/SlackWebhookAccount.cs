using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections.Slack;

public class SlackWebhookAccount : AbstractNetworkAccount
{
    public SlackWebhookAccount(string accountPrefix, IDictionary<NetworkCredentialType, string?>? credentials = null) : base(accountPrefix, credentials)
    {
    }

    public override SocialNetwork SocialNetwork => SocialNetwork.SlackWebhook;

    public override IEnumerable<NetworkCredentialType> AcceptedCredentials => 
    [
        NetworkCredentialType.IncomingWebhookUrl,

    ];

    public override IEnumerable<NetworkCredentialType> RequiredCredentials => 
    [
        NetworkCredentialType.IncomingWebhookUrl,
    ];
}