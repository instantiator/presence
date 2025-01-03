namespace Presence.Posting.Lib.Connections;

public interface INetworkCredentials : IDictionary<NetworkCredentialType, string>
{
    public string Prefix { get; }
    public (bool, IEnumerable<string>) Validate();
}