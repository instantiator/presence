using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.Thread.Composition;

public class SimpleThreadComposer : AbstractThreadComposer
{
    public static ThreadCompositionRules SIMPLE_THREAD_COMPOSITION_RULES = new ThreadCompositionRules
    {
        OnlyCountThreads = true,
        TagsOnAllPosts = true,
        TagsOnFirstPost = false,
        PostCounterPrefix = true,
        PostCounterSuffix = false,
        MaxImagesPerPost = 4,
        ImageOverflowRule = ImageOverflowRule.OverflowIntoNextPost,
        ImageOverflowText = "(continued...)",
    };

    public static PostRenderRules SIMPLE_POST_RENDER_RULES = new PostRenderRules
    {
        MaxLength = 100,
        WordSpace = " ",
        PrefixToMainJoin = " ",
        MainToSuffixJoin = "\n",
        MinAcceptableSpace = 10,
        ShowLinkUrls = false,
        SplitSnippetTextOn = [' ', '\n'],
        TruncationMark = "…"
    };

    public ICounterCreator? CounterCreator { get; protected set; }

    public SimpleThreadComposer(SocialNetwork network = SocialNetwork.Console, ThreadCompositionRules? threadRules = null, PostRenderRules? postRules = null, ICounterCreator? counterCreator = null)
        : base(network, threadRules ?? SIMPLE_THREAD_COMPOSITION_RULES, postRules ?? SIMPLE_POST_RENDER_RULES)
    {
        CounterCreator = counterCreator;
    }

    public override SocialSnippet CreatePostCounter(int index)
    {
        return new SocialSnippet()
        {
            Text = CounterCreator?.CreatePostCounter(index, ThreadRules, PostRules)
                ?? (ThreadRules.PostCounterPrefix ? $"{index + 1}." : $"/{index + 1}"),
            SnippetType = SnippetType.Counter,
        };
    }

}