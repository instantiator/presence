using System.Diagnostics.CodeAnalysis;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

[SuppressMessage("Naming", "CA1710", Justification = "An account is a set of credentials; a Dictionary/Collection suffix would obscure the domain concept.")]
public interface INetworkAccount : IDictionary<NetworkCredentialType, string?>
{
    public SocialNetwork SocialNetwork { get; }
    public string AccountPrefix { get; }
    public (bool, IEnumerable<string>) Validate();
}