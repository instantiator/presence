namespace Presence.SocialFormat.Lib.IO;

public class ThreadCompositionRequestParserRules
{
    public bool CheckLinks { get; init; }

    public static ThreadCompositionRequestParserRules Default =>
        new ThreadCompositionRequestParserRules
        {
            CheckLinks = true
        };
}