namespace SocialFormat.Lib.Composition;

public readonly record struct ThreadCompositionRules
{
    public required bool TagsOnAllPosts { get; init; }
    public required bool TagsOnFirstPost { get; init; }
    public required bool PostCounterPrefix { get; init; }
    public required bool PostCounterSuffix { get; init; }
    public required bool OnlyCountThreads { get; init; }
}
