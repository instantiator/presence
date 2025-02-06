using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.Thread.Composition;

public interface IThreadComposer
{
    ThreadComposerIdentity Identity { get; }
    PostRenderRules PostRules { get; }
    ThreadCompositionRules ThreadRules { get; }
    ComposedThread Compose(ThreadCompositionRequest request);
}