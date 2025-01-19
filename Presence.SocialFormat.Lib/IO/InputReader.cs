using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.IO.Text;

namespace Presence.SocialFormat.Lib.IO;

public class InputReader
{
    public static InputFormat? DetectFormat(string? path)
    {
        var ext = path?.Split('.').LastOrDefault()?.Trim().ToLower();
        return ext switch
        {
            null => null,
            "json" => InputFormat.JSON,
            "md" => InputFormat.MD,
            _ => throw new NotSupportedException($"Input format not recognised: {ext}")
        };
    }

    public static ThreadCompositionRequest Decode(InputFormat format, string? path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? DecodeStdIn(format)
            : DecodeInputFile(format, path);
    }

    public static ThreadCompositionRequest DecodeInputFile(InputFormat format, string path)
    {
        return format switch
        {
            InputFormat.JSON => ReadInputFileJson<ThreadCompositionRequest>(path),
            InputFormat.MD => ReadInputFile(path, new MarkdownFormatParser()),
            _ => throw new NotSupportedException($"Input format not supported: {format}")
        };
    }

    public static ThreadCompositionRequest DecodeStdIn(InputFormat format)
    {
        return format switch
        {
            InputFormat.JSON => ReadStdInJson<ThreadCompositionRequest>(),
            InputFormat.MD => ReadStdIn(new MarkdownFormatParser()),
            _ => throw new NotSupportedException($"Input format not supported: {format}")
        };
    }

    public static ThreadCompositionRequest ReadInputFile(string path, IFormatParser parser)
    {
        var content = File.ReadAllText(path);
        return parser.ToRequest(content);
    }

    public static ThreadCompositionRequest ReadStdIn(IFormatParser parser)
    {
        var input = new List<string>();
        string? line;
        while ((line = Console.ReadLine()) != null) input.Add(line);
        return parser.ToRequest(string.Join("\n", input));
    }

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