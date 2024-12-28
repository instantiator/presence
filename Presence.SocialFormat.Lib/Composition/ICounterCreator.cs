using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Composition;

public interface ICounterCreator
{
    string CreatePostCounter(int index, ThreadCompositionRules threadRules, PostRenderRules postRules);
}
