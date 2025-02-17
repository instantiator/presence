using Presence.SocialFormat.Lib.Networks.Slack;
using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class SlackThreadComposerTests
{
    [TestMethod]
    public void SlackThreadComposer_PostsHave_40000CharLimit_and_Counters()
    {
        var composer = new SlackThreadComposer();

        for (int i = 1; i < 8; i++)
        {
            // form a long snippet
            var snippet = new SocialSnippet()
            {
                Text = string.Join(' ', Enumerable.Repeat("0123456789", i*1000)),
                SnippetType = SnippetType.Text
            };

            var thread = composer.Compose(new Lib.DTO.ThreadCompositionRequest()
            {
                Message = [snippet],
                Tags = []
            });

            Assert.IsTrue(thread.Posts.Count() > 0);
            Assert.IsTrue(thread.Posts.All(p => p.ComposeText().Length <= 40000));
            if (thread.Posts.Count() > 1)
            {
                Assert.IsTrue(thread.Posts.All(p => p.Prefix.Count() == 1));
                Assert.IsTrue(thread.Posts.All(p => p.Prefix.Single().SnippetType == SnippetType.Counter));
            }
            else
            {
                Assert.IsTrue(thread.Posts.All(p => p.Prefix.Count() == 0));
            }
        }
    }
}
