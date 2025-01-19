namespace Presence.SocialFormat.Lib.Thread.Composition;

public enum ImageOverflowRule
{
    DropImages, OverflowIntoNextPost, OverflowInsertNewPost
}

public readonly record struct ThreadCompositionRules
{
    public required bool TagsOnAllPosts { get; init; }
    public required bool TagsOnFirstPost { get; init; }
    public required bool PostCounterPrefix { get; init; }
    public required bool PostCounterSuffix { get; init; }
    public required bool OnlyCountThreads { get; init; }
    public required int MaxImagesPerPost { get; init; }
    public required ImageOverflowRule ImageOverflowRule { get; init; } 
    public required string ImageOverflowText { get; init; }
}
