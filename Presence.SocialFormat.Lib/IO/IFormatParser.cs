using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.IO;

public interface IFormatParser
{
    ThreadCompositionRequest ToRequest(string str);
}
