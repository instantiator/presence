using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;
using Presence.SocialFormat.Lib.Thread.Builder;
using Presence.SocialFormat.Lib.Thread.Composition;
using Presence.SocialFormat.Tests.Composition.Helpers;

namespace Presence.SocialFormat.Tests;

[TestClass]
[TestCategory("Unit")]
public class ThreadBuilderTests
{
    [TestMethod]
    public void ThreadBuilder_RequiresComposer()
    {
        var builder = new ThreadBuilder();
        Assert.ThrowsException<ArgumentException>(() => builder.Build());
    }

    [TestMethod]
    public void ThreadBuilder_CanBuildSimpleThread()
    {
        var posts = new ThreadBuilder()
            .WithComposer(TestThreadComposer.Simple())
            .WithMessage(new SocialSnippet("Hello, world!"))
            .Build();

        Assert.AreEqual(1, posts.Count());
        Assert.AreEqual(typeof(SimpleThreadComposer), posts.Single().Key.GetType());
        Assert.AreEqual(1, posts.Single().Value.Count());
        Assert.AreEqual("Hello, world!", posts.Single().Value.Single().Message.Single().Text);
        Assert.AreEqual(SnippetType.Text, posts.Single().Value.Single().Message.Single().SnippetType);
    }

    [TestMethod]
    public void ThreadBuilder_CanBuildATThread()
    {
        var posts = new ThreadBuilder(SocialNetwork.AT)
            .WithMessage(new SocialSnippet("Hello, world!"))
            .Build();

        Assert.AreEqual(1, posts.Count());
        Assert.AreEqual(typeof(ATThreadComposer), posts.Single().Key.GetType());
        Assert.AreEqual(1, posts.Single().Value.Count());
        Assert.AreEqual("Hello, world!", posts.Single().Value.Single().Message.Single().Text);
        Assert.AreEqual(SnippetType.Text, posts.Single().Value.Single().Message.Single().SnippetType);
    }

    [TestMethod]
    public void ThreadBuilder_CanBuildMultipleThreads()
    {
        var threads = new ThreadBuilder()
            .WithComposer(SocialNetwork.AT)
            .WithComposer(SocialNetwork.Console)
            .WithMessage(new SocialSnippet("Hello, world!"))
            .Build();

        Assert.AreEqual(2, threads.Count());
        Assert.IsTrue(threads.Keys.Any(c => c.GetType() == typeof(ATThreadComposer)));
        Assert.IsTrue(threads.Keys.Any(c => c.GetType() == typeof(ConsoleThreadComposer)));
        Assert.IsTrue(threads.Values.All(p => p.Count() == 1));

        Assert.IsTrue(threads.Values.All(posts => posts.Single().Message.Single().Text == "Hello, world!"));
        Assert.IsTrue(threads.Values.All(posts => posts.Single().Message.Single().SnippetType == SnippetType.Text));
    }

}