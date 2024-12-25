using SocialFormat.Lib.Composition;
using SocialFormat.Lib.Posts;

namespace SocialFormat.Tests.Composition.Helpers;

public class TestThreadComposer
{
    public static IThreadComposer Simple() =>
        new SimpleThreadComposer(new ThreadCompositionRules
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

    public static IThreadComposer SimpleWithCounterPrefix() =>
        new SimpleThreadComposer(new ThreadCompositionRules
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

    public static SimpleThreadComposer SimpleWithoutCounters() =>
        new SimpleThreadComposer(new ThreadCompositionRules
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