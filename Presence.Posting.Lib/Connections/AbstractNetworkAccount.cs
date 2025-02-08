using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public abstract class AbstractNetworkAccount : Dictionary<NetworkCredentialType, string?>, INetworkAccount
{
    protected AbstractNetworkAccount(string accountPrefix, IDictionary<NetworkCredentialType, string?>? credentials = null)
    {
        AccountPrefix = accountPrefix;
        SetCredentials(credentials ?? new Dictionary<NetworkCredentialType, string?>());
    }

    public string AccountPrefix { get; init; }
    public abstract SocialNetwork SocialNetwork { get; }
    public abstract IEnumerable<NetworkCredentialType> AcceptedCredentials { get; }
    public abstract IEnumerable<NetworkCredentialType> RequiredCredentials { get; }

    public (bool, IEnumerable<string>) Validate()
    {
        var unexpected = Keys.Except(AcceptedCredentials);
        var missing = RequiredCredentials.Except(Keys);
        var errors = new List<string>();
        errors.AddRange(unexpected.Select(k => $"Unexpected credential: {k}"));
        errors.AddRange(missing.Select(k => $"Missing credential: {k}"));
        return (errors.Count() == 0, errors);
    }

    private void SetCredentials(IDictionary<NetworkCredentialType, string?> credentials)
    {
        foreach (var (key, value) in credentials)
        {
            if (AcceptedCredentials.Contains(key))
            {
                this[key] = value;
            }
            else
            {
                throw new ArgumentException($"Unsupported environment variable: {key}");
            }
        }
    }
}