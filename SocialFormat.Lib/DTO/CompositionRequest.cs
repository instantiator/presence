using SocialFormat.Lib.Posts;

namespace SocialFormat.Lib.DTO;

public class CompositionRequest
{
    public IEnumerable<SocialSnippet> Message { get; set; } = new List<SocialSnippet>();
    public IEnumerable<SocialSnippet> Tags { get; set; } = new List<SocialSnippet>();
}