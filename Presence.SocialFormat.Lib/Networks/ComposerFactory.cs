using Presence.SocialFormat.Lib.Networks.AT;
using Presence.SocialFormat.Lib.Networks.Console;
using Presence.SocialFormat.Lib.Networks.Slack;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.Networks;
public class ComposerFactory
{
    public static IThreadComposer ForNetwork(SocialNetwork network)
    {
        switch (network)
        {
            case SocialNetwork.Console:
                return new ConsoleThreadComposer();
            case SocialNetwork.AT:
                return new ATThreadComposer();
            case SocialNetwork.SlackWebhook:
                return new SlackThreadComposer();
            default:
                throw new NotImplementedException($"No composer implemented for network: {network}");
        }
    }
}

