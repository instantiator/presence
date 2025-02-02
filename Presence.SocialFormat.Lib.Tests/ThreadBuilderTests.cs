using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;
using Presence.SocialFormat.Lib.Tests.Composition.Helpers;
using Presence.SocialFormat.Lib.Thread.Builder;

namespace Presence.SocialFormat.Lib.Tests;

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
        var response = new ThreadBuilder()
            .WithComposer(TestThreadComposer.Simple())
            .WithMessage(new SocialSnippet("Hello, world!"))
            .Build();

        Assert.AreEqual(1, response.Threads.Count());
        Assert.AreEqual(SocialNetwork.Console, response.Threads.Single().Value.Identity.Network);

        Assert.AreEqual(1, response.Threads.Single().Value.Posts.Count());
        Assert.AreEqual("Hello, world!", response.Threads.Single().Value.Posts.Single().Message.Single().Text);
        Assert.AreEqual(SnippetType.Text, response.Threads.Single().Value.Posts.Single().Message.Single().SnippetType);
    }

    [TestMethod]
    public void ThreadBuilder_CanBuildATThread()
    {
        var response = new ThreadBuilder(SocialNetwork.AT)
            .WithMessage(new SocialSnippet("Hello, world!"))
            .Build();

        Assert.AreEqual(1, response.Threads.Count());
        Assert.AreEqual(SocialNetwork.AT, response.Threads.Single().Value.Identity.Network);
        Assert.AreEqual(1, response.Threads.Single().Value.Posts.Count());
        Assert.AreEqual("Hello, world!", response.Threads.Single().Value.Posts.Single().Message.Single().Text);
        Assert.AreEqual(SnippetType.Text, response.Threads.Single().Value.Posts.Single().Message.Single().SnippetType);
    }

    [TestMethod]
    public void ThreadBuilder_CanBuildMultipleThreads()
    {
        var response = new ThreadBuilder()
            .WithComposer(SocialNetwork.AT)
            .WithComposer(SocialNetwork.Console)
            .WithMessage(new SocialSnippet("Hello, world!"))
            .Build();

        Assert.AreEqual(2, response.Threads.Count());
        Assert.IsTrue(response.Threads.Values.Any(thread => thread.Identity.Network == SocialNetwork.AT));
        Assert.IsTrue(response.Threads.Values.Any(thread => thread.Identity.Network == SocialNetwork.Console));
        Assert.IsTrue(response.Threads.Values.All(thread => thread.Posts.Count() == 1));

        Assert.IsTrue(response.Threads.Values.All(thread => thread.Posts.Single().Message.Single().Text == "Hello, world!"));
        Assert.IsTrue(response.Threads.Values.All(thread => thread.Posts.Single().Message.Single().SnippetType == SnippetType.Text));
    }

}