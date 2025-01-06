using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.DTO;

public class ThreadCompositionRequest
{
    public IEnumerable<SocialSnippet> Message { get; set; } = new List<SocialSnippet>();
    public IEnumerable<SocialSnippet> Tags { get; set; } = new List<SocialSnippet>();
}