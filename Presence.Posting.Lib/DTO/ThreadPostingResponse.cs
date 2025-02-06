using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.DTO;

public class ThreadPostingResponse
{
    public bool FullSuccess => Summaries != null && Summaries.All(t => t.Value.All(d => d.Value.Success));
    public required Dictionary<string, Dictionary<SocialNetwork, ThreadPostSummary>> Summaries { get; init; }
}