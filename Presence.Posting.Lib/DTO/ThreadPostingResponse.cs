namespace Presence.Posting.Lib.DTO;

public class ThreadPostingResponse
{
    public bool FullSuccess => Threads != null && Threads.All(t => t.Value.Success);
    public required Dictionary<string, ThreadPostSummary> Threads { get; init; }
}