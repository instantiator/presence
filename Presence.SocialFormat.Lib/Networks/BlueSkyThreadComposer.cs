using Presence.SocialFormat.Lib.Composition;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Networks;

public class BlueSkyThreadComposer : SimpleThreadComposer
{
    public static ThreadCompositionRules BLUESKY_THREAD_COMPOSITION_RULES = new()
    {
        TagsOnAllPosts = false,
        TagsOnFirstPost = true,
        OnlyCountThreads = true,
        PostCounterPrefix = true,
        PostCounterSuffix = false,
    };

    public static PostRenderRules BLUESKY_POST_RENDER_RULES = new()
    {
        MaxLength = 300,
        MaxImagesPerPost = 4,
        ShowLinkUrls = false, // link text can be shown
        WordSpace = " ",
        SplitSnippetTextOn = [' ', '\n'],
        PrefixToMainJoin = " ",
        MainToSuffixJoin = " ",
        TruncationMark = "…",
        MinAcceptableSpace = 10,
    };

    public BlueSkyThreadComposer(ThreadCompositionRules? threadRules = null, PostRenderRules? postRules = null, ICounterCreator? counterCreator = null) : base(
            SocialNetwork.BlueSky,
            threadRules ?? BLUESKY_THREAD_COMPOSITION_RULES,
            postRules ?? BLUESKY_POST_RENDER_RULES,
            counterCreator)
    {
    }
}