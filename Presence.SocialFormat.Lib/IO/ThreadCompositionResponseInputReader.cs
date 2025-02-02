using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO;

public class ThreadCompositionResponseInputReader
{
    public static ThreadCompositionResponse Decode(string? path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? DecodeStdIn()
            : DecodeInputFile(path);
    }

    public static ThreadCompositionResponse DecodeInputFile(string path)
    {
        return InputReader.ReadInputFileJson<ThreadCompositionResponse>(path);
    }

    public static ThreadCompositionResponse DecodeStdIn()
    {
        return InputReader.ReadStdInJson<ThreadCompositionResponse>();
    }

}