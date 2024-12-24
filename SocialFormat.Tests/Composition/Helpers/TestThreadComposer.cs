using SocialFormat.Lib.Composition;
using SocialFormat.Lib.Posts;

namespace SocialFormat.Tests.Composition.Helpers;

public class TestThreadComposer : AbstractThreadComposer
{
    public TestThreadComposer(ThreadCompositionRules threadRules, PostRenderRules postRules) : base(threadRules, postRules)
    {
    }

    public override Snippet CreatePostCounter(int index)
    {
        return new Snippet()
        {
            Text = ThreadRules.PostCounterPrefix ? $"{index + 1}." : $"/{index + 1}",
            SnippetType = SnippetType.Counter,
        };
    }

    public static TestThreadComposer Simple() =>
        new TestThreadComposer(new ThreadCompositionRules
        {
            TagsOnFirstPost = false,
            TagsOnAllPosts = true,
            PostCounterPrefix = false,
            PostCounterSuffix = true,
            OnlyCountThreads = true,
        },
        new PostRenderRules
        {
            MaxLength = 100,
            WordSpace = " ",
            PrefixToMainJoin = " ",
            MainToSuffixJoin = "\n",
            MinAcceptableSpace = 10,
            ShowLinkUrls = false,
            SplitSnippetTextOn = [' ', '\n'],
            TruncationMark = "…",
        });

    public static TestThreadComposer SimpleWithCounterPrefix() =>
        new TestThreadComposer(new ThreadCompositionRules
        {
            TagsOnFirstPost = false,
            TagsOnAllPosts = true,
            PostCounterPrefix = true,
            PostCounterSuffix = false,
            OnlyCountThreads = true,
        },
        new PostRenderRules
        {
            MaxLength = 100,
            WordSpace = " ",
            PrefixToMainJoin = " ",
            MainToSuffixJoin = "\n",
            MinAcceptableSpace = 10,
            ShowLinkUrls = false,
            SplitSnippetTextOn = [' ', '\n'],
            TruncationMark = "…",
        });

    public static TestThreadComposer SimpleWithoutCounters() =>
        new TestThreadComposer(new ThreadCompositionRules
        {
            TagsOnFirstPost = false,
            TagsOnAllPosts = true,
            PostCounterPrefix = false,
            PostCounterSuffix = false,
            OnlyCountThreads = false,
        },
        new PostRenderRules
        {
            MaxLength = 100,
            WordSpace = " ",
            PrefixToMainJoin = " ",
            MainToSuffixJoin = "\n",
            MinAcceptableSpace = 10,
            ShowLinkUrls = false,
            SplitSnippetTextOn = [' ', '\n'],
            TruncationMark = "…",
        });
}