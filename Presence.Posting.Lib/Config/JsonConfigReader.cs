using System.Text.Json;

namespace Presence.Posting.Lib.Config;

public class JsonConfigReader
{
    public static IDictionary<string, string?>? ReadJsonConfig(string? json) =>
        string.IsNullOrWhiteSpace(json)
            ? null
            : JsonSerializer.Deserialize<Dictionary<string, string?>>(json);
                // ?.Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                // .ToDictionary(kv => kv.Key, kv => kv.Value!);
}