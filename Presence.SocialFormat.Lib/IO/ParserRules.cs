namespace Presence.SocialFormat.Lib.IO;

public class ParserRules
{
    public bool CheckLinks { get; init; }

    public static ParserRules Default =>
        new ParserRules 
        {
            CheckLinks = true
        };
}