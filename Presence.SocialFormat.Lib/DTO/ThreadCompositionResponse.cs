using Presence.SocialFormat.Lib.Networks;

namespace Presence.SocialFormat.Lib.DTO;

public class ThreadCompositionResponse
{
    public bool FullSuccess => Threads != null && Threads.All(t => t.Value.Success);
    public required Dictionary<SocialNetwork, ComposedThread> Threads { get; init; }
}