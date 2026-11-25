using Presence.SocialFormat.Lib.Post;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.Networks.AT;

public class ATThreadComposer : SimpleThreadComposer
{
    public static readonly ThreadCompositionRules AtThreadCompositionRules = new()
    {
        TagsOnAllPosts = false,
        TagsOnFirstPost = true,
        OnlyCountThreads = true,
        PostCounterPrefix = true,
        PostCounterSuffix = false,
        MaxImagesPerPost = 4,
        ImageOverflowRule = ImageOverflowRule.OverflowIntoNextPost,
        ImageOverflowText = "(continued...)",
    };

    public static readonly PostRenderRules AtPostRenderRules = new()
    {
        MaxLength = 300,
        ShowLinkUrls = false, // link text can be shown
        WordSpace = " ",
        SplitSnippetTextOn = [' ', '\n'],
        PrefixToMainJoin = " ",
        MainToSuffixJoin = " ",
        TruncationMark = "…",
        MinAcceptableSpace = 10,
    };

    public ATThreadComposer(ThreadCompositionRules? threadRules = null, PostRenderRules? postRules = null, ICounterCreator? counterCreator = null) : base(
            SocialNetwork.AT,
            threadRules ?? AtThreadCompositionRules,
            postRules ?? AtPostRenderRules,
            counterCreator)
    {
    }
}