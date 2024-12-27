using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Composition;

public class SimpleThreadComposer : AbstractThreadComposer
{
    public SimpleThreadComposer(ThreadCompositionRules threadRules, PostRenderRules postRules) : base(threadRules, postRules)
    {
    }

    public override SocialSnippet CreatePostCounter(int index)
    {
        return new SocialSnippet()
        {
            Text = ThreadRules.PostCounterPrefix ? $"{index + 1}." : $"/{index + 1}",
            SnippetType = SnippetType.Counter,
        };
    }

}