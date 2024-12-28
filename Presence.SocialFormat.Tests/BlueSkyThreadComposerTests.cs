using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Tests;

[TestClass]
public class BlueSkyThreadComposerTests
{
    [TestMethod]
    public void BlueSkyThreadComposer_PostsHave_300CharLimit_and_Counters()
    {
        var composer = new BlueSkyThreadComposer();

        for (int i = 1; i < 100; i++)
        {
            // form a long snippet
            var snippet = new SocialSnippet()
            {
                Text = string.Join(' ', Enumerable.Repeat("xxx", i)),
                SnippetType = SnippetType.Text
            };

            var posts = composer.Compose(new Lib.DTO.CompositionRequest()
            {
                Message = [snippet],
                Tags = []
            });

            Assert.IsTrue(posts.Count() > 0);
            Assert.IsTrue(posts.All(p => p.ComposeText().Length <= 300));
            if (posts.Count() > 1)
            {
                Assert.IsTrue(posts.All(p => p.Prefix.Count() == 1));
                Assert.IsTrue(posts.All(p => p.Prefix.Single().SnippetType == SnippetType.Counter));
            }
            else
            {
                Assert.IsTrue(posts.All(p => p.Prefix.Count() == 0));
            }
        }
    }
}