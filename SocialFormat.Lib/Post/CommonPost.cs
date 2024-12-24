namespace SocialFormat.Lib.Posts;


public class CommonPost
{
    public CommonPost(int index, PostRenderRules rules)
    {
        Index = index;
        Rules = rules;
    }

    public int Index { get; private set; }
    public PostRenderRules Rules { get; private set; }

    public IEnumerable<SocialSnippet> Prefix { get; set; } = new List<SocialSnippet>();
    public IEnumerable<SocialSnippet> Message { get; set; } = new List<SocialSnippet>();
    public IEnumerable<SocialSnippet> Suffix { get; set; } = new List<SocialSnippet>();
    public int Length => ComposeText(Rules, Prefix, Message, Suffix).Length;
    public bool MarkedComplete { get; set; }

    public int MessageSpace =>
        MarkedComplete ? 0 :
            Rules.MaxLength -
            Length -
            (Prefix.Count() > 0 && Message.Count() == 0 ? Rules.PrefixToMainJoin.Length : 0) -
            (Suffix.Count() > 0 && Message.Count() == 0 ? Rules.MainToSuffixJoin.Length : 0);

    public bool Fits(SocialSnippet snippet) =>
        !MarkedComplete && ComposeText(Rules, Prefix, Message.Append(snippet), Suffix).Length <= Rules.MaxLength;

    public string ComposeText() => ComposeText(Rules, Prefix, Message, Suffix);

    public static string ComposeText(PostRenderRules rules, IEnumerable<SocialSnippet> prefix, IEnumerable<SocialSnippet> message, IEnumerable<SocialSnippet> suffix)
    {
        return
            string.Join(rules.WordSpace, prefix.Select(s => s.Compose(rules))) +
            (prefix.Count() > 0 && message.Count() > 0 ? rules.PrefixToMainJoin : string.Empty) +
            string.Join(rules.WordSpace, message.Select(s => s.Compose(rules))) +
            (message.Count() > 0 && suffix.Count() > 0 ? rules.MainToSuffixJoin : string.Empty) +
            string.Join(rules.WordSpace, suffix.Select(s => s.Compose(rules)));
    }

    public IList<CommonPostImage> Images { get; set; } = new List<CommonPostImage>();

}
