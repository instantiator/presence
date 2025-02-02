namespace Presence.SocialFormat.Lib.DTO;

public class ThreadCompositionResponse
{
    public bool FullSuccess => Threads != null && Threads.All(t => t.Value.Success);
    public required Dictionary<string, ComposedThread> Threads { get; init; }
}