using Presence.SocialFormat.Lib.Post;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.Networks.Console;

public class ConsoleThreadComposer : SimpleThreadComposer
{
    public ConsoleThreadComposer(ThreadCompositionRules? threadRules = null, PostRenderRules? postRules = null, ICounterCreator? counterCreator = null) : base(
            SocialNetwork.Console,
            threadRules ?? SimpleThreadCompositionRules,
            postRules ?? SimplePostRenderRules,
            counterCreator)
    {
    }
}