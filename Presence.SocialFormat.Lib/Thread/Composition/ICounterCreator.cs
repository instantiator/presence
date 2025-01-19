using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.Thread.Composition;

public interface ICounterCreator
{
    string CreatePostCounter(int index, ThreadCompositionRules threadRules, PostRenderRules postRules);
}
