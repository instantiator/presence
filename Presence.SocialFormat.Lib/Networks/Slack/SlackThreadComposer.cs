using Presence.SocialFormat.Lib.Post;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.Networks.Slack;

public class SlackThreadComposer : SimpleThreadComposer
{
    public static readonly ThreadCompositionRules SlackThreadCompositionRules = new()
    {
        TagsOnAllPosts = false,
        TagsOnFirstPost = true,
        OnlyCountThreads = true,
        PostCounterPrefix = false,
        PostCounterSuffix = true,
        MaxImagesPerPost = 4,
        ImageOverflowRule = ImageOverflowRule.OverflowIntoNextPost,
        ImageOverflowText = "(continued...)",
    };

    public static readonly PostRenderRules SlackPostRenderRules = new()
    {
        MaxLength = 40000,
        ShowLinkUrls = false, // link text can be shown
        WordSpace = " ",
        SplitSnippetTextOn = [' ', '\n'],
        PrefixToMainJoin = " ",
        MainToSuffixJoin = " ",
        TruncationMark = "…",
        MinAcceptableSpace = 10,
    };

    public SlackThreadComposer(ThreadCompositionRules? threadRules = null, PostRenderRules? postRules = null, ICounterCreator? counterCreator = null) : base(
            SocialNetwork.SlackWebhook,
            threadRules ?? SlackThreadCompositionRules,
            postRules ?? SlackPostRenderRules,
            counterCreator)
    {
    }
}