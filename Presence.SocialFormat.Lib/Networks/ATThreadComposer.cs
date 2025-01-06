using Presence.SocialFormat.Lib.Posts;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.Networks;

public class ATThreadComposer : SimpleThreadComposer
{
    public static ThreadCompositionRules AT_THREAD_COMPOSITION_RULES = new()
    {
        TagsOnAllPosts = false,
        TagsOnFirstPost = true,
        OnlyCountThreads = true,
        PostCounterPrefix = true,
        PostCounterSuffix = false,
    };

    public static PostRenderRules AT_POST_RENDER_RULES = new()
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

    public ATThreadComposer(ThreadCompositionRules? threadRules = null, PostRenderRules? postRules = null, ICounterCreator? counterCreator = null) : base(
            SocialNetwork.AT,
            threadRules ?? AT_THREAD_COMPOSITION_RULES,
            postRules ?? AT_POST_RENDER_RULES,
            counterCreator)
    {
    }
}