using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO;

public class OutputWriter
{
    public static string Encode(OutputFormat format, ThreadCompositionResponse result)
    {
        return format switch
        {
            OutputFormat.Json => JsonSerializer.Serialize(result, opts),
            OutputFormat.HumanReadable => HumanReadable(result),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unknown output format.")
        };
    }

    private static string HumanReadable(ThreadCompositionResponse response)
    {
        var separator = "---";
        var lines = new List<string>();

        if (response.Success)
        {
            foreach (var network in response.Threads!.Keys)
            {
                lines.Add($"Network: {network}");
                lines.Add(string.Join($"\n  {separator}\n", response.Threads![network].Select(p => "✉️ " + p.ComposeText())));
            }
        }
        else
        {
            lines.Add("Exception encountered processing thread.");
            lines.Add(separator);
            lines.Add($"Exception: {response.ExceptionType}");
            lines.Add($"Message: {response.ExceptionMessage}");
            lines.Add($"Stack trace: {response.ExceptionStackTrace}");
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