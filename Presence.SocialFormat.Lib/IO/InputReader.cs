using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO;

public class InputReader
{
    public static ThreadCompositionRequest ReadInputFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Input file not found: {path}", path);
        }

        var input = File.ReadAllText(path);
        return JsonSerializer.Deserialize<ThreadCompositionRequest>(input, opts)!;
    }

    public static ThreadCompositionRequest ReadStdIn()
    {
        var input = new StringBuilder();
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            input.AppendLine(line);
        }

        return JsonSerializer.Deserialize<ThreadCompositionRequest>(input.ToString(), opts)!;
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