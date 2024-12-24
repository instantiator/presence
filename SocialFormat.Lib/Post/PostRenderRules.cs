namespace SocialFormat.Lib.Posts;

public readonly struct PostRenderRules
{
    public required int MinAcceptableSpace { get; init; }
    public required int MaxLength { get; init; }
    public required bool ShowLinkUrls { get; init; }
    public required string WordSpace { get; init; }
    public required char[]? SplitSnippetTextOn { get; init; }
    public required string PrefixToMainJoin { get; init; }
    public required string MainToSuffixJoin { get; init; }
    public required string TruncationMark { get; init; }
}
