using Presence.SocialFormat.Lib.Builder;
using Presence.SocialFormat.Lib.Composition;
using Presence.SocialFormat.Lib.Posts;
using Presence.SocialFormat.Tests.Composition.Helpers;

namespace Presence.SocialFormat.Tests;

[TestClass]
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

}