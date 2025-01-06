using Presence.SocialFormat.Lib.Networks;

namespace Presence.SocialFormat.Lib.Thread.Composition
{
    public class ComposerFactory
    {
        public static IThreadComposer ForNetwork(SocialNetwork network)
        {
            switch (network)
            {
                case SocialNetwork.Test:
                    return new SimpleThreadComposer();
                case SocialNetwork.AT:
                    return new ATThreadComposer();
                default:
                    throw new NotImplementedException($"No composer implemented for network: {network}");
            }
        }
    }

}