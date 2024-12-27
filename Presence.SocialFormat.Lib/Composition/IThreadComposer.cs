using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Composition;

public interface IThreadComposer
{
    PostRenderRules PostRules { get; }
    ThreadCompositionRules ThreadRules { get; }
    IEnumerable<CommonPost> Compose(CompositionRequest request);
}