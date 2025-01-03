
using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Operators;
using System.Linq;

namespace Presence.Posting.Lib.Connections;

public abstract class AbstractNetworkCredentials : Dictionary<NetworkCredentialType, string>, INetworkCredentials
{
    protected AbstractNetworkCredentials()
    {
    }

    protected AbstractNetworkCredentials(IDictionary<NetworkCredentialType, string> credentials)
    {
        foreach (var (key, value) in credentials)
        {
            this[key] = value;
        }
    }

    protected AbstractNetworkCredentials(IDictionary? env)
    {
        if (env != null)
        {
            if (env is IDictionary<string, string> stringEnv)
            {
                SetCredentials(stringEnv);
            }
            else
            {
                var des = env.Cast<DictionaryEntry>();
                SetCredentials(env.Cast<DictionaryEntry>().ToDictionary(kv => (string)kv.Key, kv => (string)kv.Value!));
            }
        }
    }

    public abstract string Prefix { get; }

    public abstract (bool, IEnumerable<string>) Validate();

    public void SetCredentials(IDictionary<string, string> env)
    {
        foreach (var (key, value) in env)
        {
            if (key.ToLower().StartsWith(Prefix.ToLower()))
            {
                var shortKey = key.Substring(Prefix.Length); // remove prefix
                if (Enum.TryParse<NetworkCredentialType>(shortKey, true, out var type))
                {
                    this[type] = value;
                }
                else
                {
                    throw new ArgumentException($"Unsupported environment variable: {key}");
                }
            }
        }
    }

}