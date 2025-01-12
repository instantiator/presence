using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO;

public class OutputWriter
{
    public static string Encode(OutputFormat format, ThreadCompositionResponse response)
    {
        return format switch
        {
            OutputFormat.Json => JsonSerializer.Serialize(response, opts),
            OutputFormat.HumanReadable => HumanReadable(response),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unknown output format.")
        };
    }

    private static string HumanReadable(ThreadCompositionResponse response)
    {
        var separator = "---";
        var lines = new List<string>();

        foreach (var network in response.Threads!.Keys)
        {
            var thread = response.Threads![network];
            lines.Add($"Network: {network} {(thread.Success ? "✅" : "❌")}");
            lines.Add(separator);
            lines.Add(string.Join($"\n   {separator}\n", thread.Posts.Select(p => " ⏩ " + p.ComposeText())));
            if (thread.ExceptionType != null)
            {
                lines.Add($"   {separator}");
                lines.Add($" ❌ {thread.ExceptionType}: {thread.ExceptionMessage}");
            }
            lines.Add(""); // break
        }
        return string.Join("\n", lines);
    }

    // strict on unknown properties, relaxed on case sensitivity
    private static JsonSerializerOptions opts = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
        WriteIndented = true
    };

}