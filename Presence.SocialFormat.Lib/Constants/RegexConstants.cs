namespace Presence.SocialFormat.Lib.Constants;

public class RegexConstants
{
    public static string MD_LINK_REGEX = @"\!?\[(?<text>.+?)\]\((?<uri>[\S]+)\)";
    public static string TAG_REGEX = @"#(?<tag>[a-zA-Z0-9_-]+)";
}
