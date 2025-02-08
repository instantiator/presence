using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public interface INetworkAccount : IDictionary<NetworkCredentialType, string?>
{
    public SocialNetwork SocialNetwork { get; }
    public string AccountPrefix { get; }
    public (bool, IEnumerable<string>) Validate();
}