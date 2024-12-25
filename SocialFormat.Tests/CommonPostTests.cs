using SocialFormat.Lib.Posts;

namespace SocialFormat.Tests;

[TestClass]
public class CommonPostTests
{
    public static PostRenderRules SimpleRules = new PostRenderRules()
    {
        MinAcceptableSpace = 10,
        MaxLength = 100,
        ShowLinkUrls = false,
        WordSpace = " ",
        SplitSnippetTextOn = [' ', '\n'],
        PrefixToMainJoin = " ",
        MainToSuffixJoin = "\n",
        TruncationMark = "…",
    };

    public static PostRenderRules SimpleRulesShowLinks = new PostRenderRules()
    {
        MinAcceptableSpace = 10,
        MaxLength = 100,
        ShowLinkUrls = true,
        WordSpace = " ",
        SplitSnippetTextOn = [' ', '\n'],
        PrefixToMainJoin = " ",
        MainToSuffixJoin = "\n",
        TruncationMark = "…",
    };

    [TestMethod]
    public void CommonPost_WithSimpleRules_CanComposeText()
    {
        var post = new CommonPost(0, SimpleRules);

        post.Prefix = new List<SocialSnippet>
        {
            new SocialSnippet
            {
                Text = "1.",
                SnippetType = SnippetType.Counter,
            }
        };

        post.Message = new List<SocialSnippet>
        {
            new SocialSnippet
            {
                Text = "Part 1.",
                SnippetType = SnippetType.Text,
            },
            new SocialSnippet
            {
                Text = "Part 2.",
                SnippetType = SnippetType.Text,
            }
        };

        post.Suffix = new List<SocialSnippet>
        {
            new SocialSnippet
            {
                Text = "#TagOne",
                SnippetType = SnippetType.Tag,
            },
            new SocialSnippet
            {
                Text = "#TagTwo",
                SnippetType = SnippetType.Tag,
            }
        };

        var text = post.ComposeText();
        Assert.IsNotNull(text);
        Assert.AreEqual("1. Part 1. Part 2.\n#TagOne #TagTwo", text);
    }

    [TestMethod]
    public void CommonPost_WithSimpleRules_CanComposeLinks()
    {
        var message = new List<SocialSnippet>
            {
                new SocialSnippet
                {
                    Text = "Text",
                    SnippetType = SnippetType.Text,
                },
                new SocialSnippet
                {
                    Text = "Link",
                    Reference = "https://example.com",
                    SnippetType = SnippetType.Link,
                }
            };

        var post1 = new CommonPost(0, SimpleRules) { Message = message };
        var text1 = post1.ComposeText();
        Assert.IsNotNull(text1);
        Assert.AreEqual("Text Link", text1);

        var post2 = new CommonPost(0, SimpleRulesShowLinks) { Message = message };
        var text2 = post2.ComposeText();
        Assert.IsNotNull(text2);
        Assert.AreEqual("Text https://example.com", text2);

    }

}