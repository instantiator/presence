using System.Collections;

namespace Presence.Posting.Lib.Helpers;

public static class DictionaryExtension
{
    public static IDictionary<string,string?> ToStringDictionary(this IDictionary dict)
        => dict is IDictionary<string, string?> strings 
            ? strings 
            : dict.Cast<DictionaryEntry>().ToDictionary(kv => (string)kv.Key, kv => (string?)kv.Value);
}