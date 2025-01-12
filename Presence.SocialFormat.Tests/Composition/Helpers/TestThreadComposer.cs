using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Tests.Composition.Helpers;

public class TestThreadComposer
{
    public static IThreadComposer Simple() =>
        new SimpleThreadComposer(
            SocialNetwork.Console,
            new ThreadCompositionRules
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
            MaxImagesPerPost = 4,
            WordSpace = " ",
            PrefixToMainJoin = " ",
            MainToSuffixJoin = "\n",
            MinAcceptableSpace = 10,
            ShowLinkUrls = false,
            SplitSnippetTextOn = [' ', '\n'],
            TruncationMark = "…",
        });

    public static IThreadComposer SimpleWithCounterPrefix() =>
        new SimpleThreadComposer(
            SocialNetwork.Console,
            new ThreadCompositionRules
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
            MaxImagesPerPost = 4,
            WordSpace = " ",
            PrefixToMainJoin = " ",
            MainToSuffixJoin = "\n",
            MinAcceptableSpace = 10,
            ShowLinkUrls = false,
            SplitSnippetTextOn = [' ', '\n'],
            TruncationMark = "…",
        });

    public static SimpleThreadComposer SimpleWithoutCounters() =>
        new SimpleThreadComposer(
            SocialNetwork.Console,
            new ThreadCompositionRules
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
            MaxImagesPerPost = 4,
            WordSpace = " ",
            PrefixToMainJoin = " ",
            MainToSuffixJoin = "\n",
            MinAcceptableSpace = 10,
            ShowLinkUrls = false,
            SplitSnippetTextOn = [' ', '\n'],
            TruncationMark = "…",
        });
}