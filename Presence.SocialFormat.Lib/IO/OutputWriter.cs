using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.IO.Text;

namespace Presence.SocialFormat.Lib.IO;

public class OutputWriter
{
    public static string Encode(OutputFormat format, ThreadCompositionResponse response)
    {
        return format switch
        {
            OutputFormat.Json => JsonSerializer.Serialize(response, opts),
            OutputFormat.HR => new HumanReadableWriter().ToString(response),
            OutputFormat.Markdown => new MarkdownFormatWriter().ToString(response),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unknown output format.")
        };
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