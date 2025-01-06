using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Composition;

public interface IThreadComposer
{
    SocialNetwork Network { get; }
    PostRenderRules PostRules { get; }
    ThreadCompositionRules ThreadRules { get; }
    IEnumerable<CommonPost> Compose(ThreadCompositionRequest request);
}