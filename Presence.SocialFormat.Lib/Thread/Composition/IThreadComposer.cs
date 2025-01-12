using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Thread.Composition;

public interface IThreadComposer
{
    ThreadComposerIdentity Identity { get; }
    PostRenderRules PostRules { get; }
    ThreadCompositionRules ThreadRules { get; }
    ComposedThread Compose(ThreadCompositionRequest request);
}