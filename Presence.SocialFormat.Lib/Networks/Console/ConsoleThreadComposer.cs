using Presence.SocialFormat.Lib.Post;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.Networks.Console;

public class ConsoleThreadComposer : SimpleThreadComposer
{
    public ConsoleThreadComposer(ThreadCompositionRules? threadRules = null, PostRenderRules? postRules = null, ICounterCreator? counterCreator = null) : base(
            SocialNetwork.Console,
            threadRules ?? SIMPLE_THREAD_COMPOSITION_RULES,
            postRules ?? SIMPLE_POST_RENDER_RULES,
            counterCreator)
    {
    }
}