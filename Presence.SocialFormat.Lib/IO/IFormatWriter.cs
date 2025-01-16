using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO;

public interface IFormatWriter
{
    string ToString(ThreadCompositionResponse response);
}