using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO;

public interface IFormatParser
{
    ThreadCompositionRequest ToRequest(string str);
}
