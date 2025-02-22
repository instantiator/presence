using Presence.SocialFormat.Lib.Networks.AT;
using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class ATThreadComposerTests
{
    [TestMethod]
    public void ATThreadComposer_PostsHave_300CharLimit_and_Counters()
    {
        var composer = new ATThreadComposer();

        for (int i = 1; i < 6; i++)
        {
            // form a long snippet
            var snippet = new SocialSnippet()
            {
                Text = string.Join(' ', Enumerable.Repeat("0123456789", i*100)),
                SnippetType = SnippetType.Text
            };

            var thread = composer.Compose(new Lib.DTO.ThreadCompositionRequest()
            {
                Message = [snippet],
                Tags = []
            });

            Assert.IsTrue(thread.Posts.Count() > 0);
            Assert.IsTrue(thread.Posts.All(p => p.ComposeText().Length <= 300));
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