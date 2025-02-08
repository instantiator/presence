using System.Collections;
using Presence.Posting.Lib.Constants;
using Presence.Posting.Lib.Helpers;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Config;

public class EnvironmentConfigReader : Dictionary<string, Dictionary<SocialNetwork, Dictionary<NetworkCredentialType, string?>>>
{
    public EnvironmentConfigReader(IDictionary env)
    {
        // If the JSON data key is present use that in preference to other environment variables.
        // Otherwise, convert the the environment dictionary to a simple string dictionary.
        var strings = (env.Contains(ConfigKeys.JSON_DATA_ENV_KEY)
            ? JsonConfigReader.ReadJsonConfig(env[ConfigKeys.JSON_DATA_ENV_KEY] as string)
            : null) ?? env.ToStringDictionary();

        // extract prefixes
        var prefixes = strings.ContainsKey(ConfigKeys.ACCOUNTS_ENV_KEY) && !string.IsNullOrWhiteSpace(strings[ConfigKeys.ACCOUNTS_ENV_KEY])
            ? strings[ConfigKeys.ACCOUNTS_ENV_KEY]!.Split(',')
            : throw new ArgumentException($"Please provide a comma separated list of account prefixes in config key: {ConfigKeys.ACCOUNTS_ENV_KEY}");

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

    private Dictionary<NetworkCredentialType,string?> ExtractCredentials(string prefix, SocialNetwork network, IDictionary<string,string?> env)
    => env
        .ToDictionary(kv => kv.Key.Trim(), kv => kv.Value?.Trim())
        .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        .ToDictionary(kv => kv.Key.Substring(prefix.Length).Trim('_'), kv => kv.Value)
        .Where(kv => kv.Key.StartsWith(network.ToString(), StringComparison.OrdinalIgnoreCase))
        .ToDictionary(kv => kv.Key.Substring(network.ToString().Length).Trim('_'), kv => kv.Value)
        .ToDictionary(kv => Enum.Parse<NetworkCredentialType>(kv.Key, true), kv => kv.Value);

}