using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Presence.SocialFormat.Lib.IO;

public class InputReader
{
    public static T ReadInputFileJson<T>(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Input file not found: {path}", path);
        }

        var input = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(input, opts)!;
    }

    public static T ReadStdInJson<T>()
    {
        var input = new StringBuilder();
        string? line;
        while ((line = Console.ReadLine()) != null) input.AppendLine(line);
        return JsonSerializer.Deserialize<T>(input.ToString(), opts)!;
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