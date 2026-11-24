namespace Presence.SocialFormat.Lib.Constants;

/// <summary>Regular expression patterns for the markdown and tag syntax Presence understands.</summary>
public static class RegexConstants
{
    /// <summary>A markdown link: <c>[text](uri)</c>.</summary>
    public const string MdLinkRegex = @"\[(?<text>.+?)\]\((?<uri>[\S]+)\)";
    /// <summary>A markdown image: <c>![alt](uri)</c>.</summary>
    public const string MdImageRegex = @"\!\[(?<text>.+?)\]\((?<uri>[\S]+)\)";
    /// <summary>A hashtag: <c>#tag</c>.</summary>
    public const string TagRegex = @"#(?<tag>[a-zA-Z0-9_-]+)";
}
