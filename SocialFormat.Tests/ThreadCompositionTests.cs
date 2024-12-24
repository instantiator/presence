using SocialFormat.Lib.Posts;
using SocialFormat.Tests.Composition.Helpers;

namespace SocialFormat.Tests;

[TestClass]
public class ThreadCompositionTests
{
    [TestMethod]
    public void SimpleThreadComposer_GivenSimpleMessage_ComposesSinglePost()
    {
        var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "Hello, world!",
                SnippetType = SnippetType.Text
            }
        };

        var composer = TestThreadComposer.Simple();
        var posts = composer.Compose(message, []);
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual(1, posts.Count());
        Assert.AreEqual("Hello, world!", posts.Single().ComposeText());
    }

    [TestMethod]
    public void SimpleThreadComposer_GivenLongSimpleMessage_ComposesMultiplePosts()
    {
        var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "This is a very long text snippet that ought to get split across two posts. The split comes around here and this is almost certainly in the next post.",
                SnippetType = SnippetType.Text
            }
        };

        var composer = TestThreadComposer.Simple();
        var posts = composer.Compose(message, []);
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual(2, posts.Count());
        Assert.AreEqual("This is a very long text snippet that ought to get split across two posts. The split comes\n/1", posts.ElementAt(0).ComposeText());
        Assert.AreEqual("around here and this is almost certainly in the next post.\n/2", posts.ElementAt(1).ComposeText());
    }

    [TestMethod]
    public void SimpleThreadComposer_GivenMultipleSnippets_ComposesSinglePost()
    {
        var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "Hello, world!",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This is a multi-snippet message.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "With 3 snippets.",
                SnippetType = SnippetType.Text
            }
        };

        var composer = TestThreadComposer.Simple();
        var posts = composer.Compose(message, []);
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual(1, posts.Count());
        Assert.AreEqual("Hello, world! This is a multi-snippet message. With 3 snippets.", posts.Single().ComposeText());
    }

   [TestMethod]
    public void SimpleThreadComposer_GivenMultipleSnippetsAndBreak_ComposesMultiplePosts()
    {
        var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "Hello, world!",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                SnippetType = SnippetType.Break
            },
            new Snippet
            {
                Text = "This is a multi-snippet message.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "With 3 snippets.",
                SnippetType = SnippetType.Text
            }
        };

        var composer = TestThreadComposer.Simple();
        var posts = composer.Compose(message, []);
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual(2, posts.Count());
        Assert.AreEqual("Hello, world!\n/1", posts.ElementAt(0).ComposeText());
        Assert.AreEqual("This is a multi-snippet message. With 3 snippets.\n/2", posts.ElementAt(1).ComposeText());
    }

    [TestMethod]
    public void SimpleThreadComposer_GivenMultipleSnippets_ComposesMultiplePosts()
    {
         var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "This is a very long multi-snippet message. It should get split across several posts.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "Each post is limited to 100 characters, and this includes the post counter.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "The post counter appears when there is more than 1 post in the thread.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This test deals with only text-type posts for now.",
                SnippetType = SnippetType.Text
            }
        };

        var composer = TestThreadComposer.Simple();
        var posts = composer.Compose(message, []);
        Assert.AreEqual(3, posts.Count());
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual("This is a very long multi-snippet message. It should get split across several posts. Each post is\n/1", posts.ElementAt(0).ComposeText());
        Assert.AreEqual("limited to 100 characters, and this includes the post counter. The post counter appears when\n/2", posts.ElementAt(1).ComposeText());
        Assert.AreEqual("there is more than 1 post in the thread. This test deals with only text-type posts for now.\n/3", posts.ElementAt(2).ComposeText());
    }

    [TestMethod]
    public void SimpleThreadComposer_WithoutCounters_GivenMultipleSnippets_ComposesMultiplePostsWithoutCounters()
    {
         var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "This is a very long multi-snippet message. It should get split across several posts.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "Each post is limited to 100 characters, and this includes the post counter.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "The post counter appears when there is more than 1 post in the thread.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This test deals with only text-type posts for now.",
                SnippetType = SnippetType.Text
            }
        };

        var composer = TestThreadComposer.SimpleWithoutCounters();
        var posts = composer.Compose(message, []);
        Assert.AreEqual(3, posts.Count());
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual("This is a very long multi-snippet message. It should get split across several posts. Each post is", posts.ElementAt(0).ComposeText());
        Assert.AreEqual("limited to 100 characters, and this includes the post counter. The post counter appears when there", posts.ElementAt(1).ComposeText());
        Assert.AreEqual("is more than 1 post in the thread. This test deals with only text-type posts for now.", posts.ElementAt(2).ComposeText());
    }

    [TestMethod]
    public void SimpleThreadComposer_GivenMultipleSnippetsAndTags_ComposesSinglePost()
    {
        var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "Hello, world!",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This is a multi-snippet message.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "With 3 snippets.",
                SnippetType = SnippetType.Text
            }
        };

        var tags = new List<Snippet> {
            new Snippet
            {
                Text = "#TagOne",
                SnippetType = SnippetType.HashTag
            },
            new Snippet
            {
                Text = "#TagTwo",
                SnippetType = SnippetType.HashTag
            }
        };

        var composer = TestThreadComposer.Simple();
        var posts = composer.Compose(message, tags);
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual(1, posts.Count());
        Assert.AreEqual(2, posts.Single().Suffix.Count());
        Assert.AreEqual("Hello, world! This is a multi-snippet message. With 3 snippets.\n#TagOne #TagTwo", posts.Single().ComposeText());
    }

    [TestMethod]
    public void SimpleThreadComposer_GivenMultipleSnippetsAndTags_ComposesMultiplePosts()
    {
        var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "Hello, world!",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This is a multi-snippet message.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "With 6 snippets.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This is snippet 4.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This is snippet 5.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This is snippet 6 (the last snippet).",
                SnippetType = SnippetType.Text
            }
        };

        var tags = new List<Snippet> {
            new Snippet
            {
                Text = "#TagOne",
                SnippetType = SnippetType.HashTag
            },
            new Snippet
            {
                Text = "#TagTwo",
                SnippetType = SnippetType.HashTag
            }
        };

        var composer = TestThreadComposer.Simple();
        var posts = composer.Compose(message, tags);
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual(2, posts.Count());
        Assert.IsTrue(posts.All(p => p.Suffix.Count(s => s.SnippetType == SnippetType.HashTag) == 2));
        Assert.IsTrue(posts.All(p => p.Suffix.Count(s => s.SnippetType == SnippetType.Counter) == 1));
        Assert.AreEqual("Hello, world! This is a multi-snippet message. With 6 snippets. This is snippet\n/1 #TagOne #TagTwo", posts.ElementAt(0).ComposeText());
        Assert.AreEqual("4. This is snippet 5. This is snippet 6 (the last snippet).\n/2 #TagOne #TagTwo", posts.ElementAt(1).ComposeText());
    }

    [TestMethod]
    public void SimpleThreadComposer_WithCounterPrefix_GivenMultipleSnippets_ComposesMultiplePosts()
    {
         var message = new List<Snippet>
        {
            new Snippet
            {
                Text = "This is a very long multi-snippet message. It should get split across several posts.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "Each post is limited to 100 characters, and this includes the post counter.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "The post counter appears when there is more than 1 post in the thread.",
                SnippetType = SnippetType.Text
            },
            new Snippet
            {
                Text = "This test deals with only text-type posts for now.",
                SnippetType = SnippetType.Text
            }
        };

        var composer = TestThreadComposer.SimpleWithCounterPrefix();
        var posts = composer.Compose(message, []);
        Assert.AreEqual(3, posts.Count());
        Assert.IsTrue(posts.All(p => p.ComposeText().Length <= composer.PostRules.MaxLength));
        Assert.AreEqual("1. This is a very long multi-snippet message. It should get split across several posts. Each post is", posts.ElementAt(0).ComposeText());
        Assert.AreEqual("2. limited to 100 characters, and this includes the post counter. The post counter appears when", posts.ElementAt(1).ComposeText());
        Assert.AreEqual("3. there is more than 1 post in the thread. This test deals with only text-type posts for now.", posts.ElementAt(2).ComposeText());
    }

}
