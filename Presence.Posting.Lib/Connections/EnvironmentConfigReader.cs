using System.Collections;
using Presence.Posting.Lib.Helpers;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public class EnvironmentConfigReader : Dictionary<string, Dictionary<SocialNetwork, Dictionary<NetworkCredentialType, string>>>
{
    public const string ACCOUNTS_ENV_KEY = "PRESENCE_ACCOUNTS";

    public EnvironmentConfigReader(IDictionary env)
    {
        // extract account prefixes
        var strings = env.ToStringDictionary();
        var prefixes = strings[ACCOUNTS_ENV_KEY].Split(',');

        // extract credentials per network per prefix
        var credentials = prefixes
            .ToDictionary(
                prefix => prefix, 
                prefix => 
                    strings.Keys
                        .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        .Select(k => k.Substring($"{prefix}_".Length))
                        .Select(k => Enum.GetValues<SocialNetwork>().Where(n => k.StartsWith(n.ToString(), StringComparison.OrdinalIgnoreCase)))
                        .Where(nn => nn.Count() != 0)
                        .Select(nn => nn.Single())
                        .Distinct()
                        .ToDictionary(
                            network => network,
                            network => ExtractCredentials(prefix, network, strings)));

        // initialise as a dictionary
        foreach (var (prefix, creds) in credentials)
        {
            this[prefix] = creds;
        }
    }

    private Dictionary<NetworkCredentialType,string> ExtractCredentials(string prefix, SocialNetwork network, IDictionary<string,string> env)
    => env
        .ToDictionary(kv => kv.Key.Trim(), kv => kv.Value.Trim())
        .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        .ToDictionary(kv => kv.Key.Substring(prefix.Length).Trim('_'), kv => kv.Value)
        .Where(kv => kv.Key.StartsWith(network.ToString(), StringComparison.OrdinalIgnoreCase))
        .ToDictionary(kv => kv.Key.Substring(network.ToString().Length).Trim('_'), kv => kv.Value)
        .ToDictionary(kv => Enum.Parse<NetworkCredentialType>(kv.Key, true), kv => kv.Value);

}