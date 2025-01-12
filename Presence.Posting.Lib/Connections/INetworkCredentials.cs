using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public interface INetworkCredentials : IDictionary<NetworkCredentialType, string>
{
    public SocialNetwork SocialNetwork { get; init; }
    public string Prefix { get; }
    public (bool, IEnumerable<string>) Validate();
}