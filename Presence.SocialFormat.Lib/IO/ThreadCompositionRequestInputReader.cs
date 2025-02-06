using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.IO.Text;

namespace Presence.SocialFormat.Lib.IO;

public class ThreadCompositionRequestInputReader
{
    public static ThreadCompositionRequestInputFormat? DetectFormat(string? path)
    {
        var ext = path?.Split('.').LastOrDefault()?.Trim().ToLower();
        return ext switch
        {
            null => null,
            "json" => ThreadCompositionRequestInputFormat.JSON,
            "md" => ThreadCompositionRequestInputFormat.MD,
            _ => throw new NotSupportedException($"Input format not recognised: {ext}")
        };
    }

    public static ThreadCompositionRequest Decode(ThreadCompositionRequestInputFormat format, string? path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? DecodeStdIn(format)
            : DecodeInputFile(format, path);
    }

    public static ThreadCompositionRequest DecodeInputFile(ThreadCompositionRequestInputFormat format, string path)
    {
        return format switch
        {
            ThreadCompositionRequestInputFormat.JSON => InputReader.ReadInputFileJson<ThreadCompositionRequest>(path),
            ThreadCompositionRequestInputFormat.MD => ReadInputFile(path, new MarkdownFormatParser()),
            _ => throw new NotSupportedException($"Input format not supported: {format}")
        };
    }

    public static ThreadCompositionRequest DecodeStdIn(ThreadCompositionRequestInputFormat format)
    {
        return format switch
        {
            ThreadCompositionRequestInputFormat.JSON => InputReader.ReadStdInJson<ThreadCompositionRequest>(),
            ThreadCompositionRequestInputFormat.MD => ReadStdIn(new MarkdownFormatParser()),
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
}