namespace SocialFormat.Lib.Posts;


public class Post
{
    public Post(int index, PostRenderRules rules)
    {
        Index = index;
        Rules = rules;
    }

    public int Index { get; private set; }
    public PostRenderRules Rules { get; private set; }

    public IEnumerable<Snippet> Prefix { get; set; } = new List<Snippet>();
    public IEnumerable<Snippet> Message { get; set; } = new List<Snippet>();
    public IEnumerable<Snippet> Suffix { get; set; } = new List<Snippet>();
    public int Length => ComposeText(Rules, Prefix, Message, Suffix).Length;
    public bool MarkedComplete { get; set; }

    public int MessageSpace => 
        MarkedComplete ? 0 :
            Rules.MaxLength - 
            Length - 
            (Prefix.Count() > 0 && Message.Count() == 0 ? Rules.PrefixToMainJoin.Length : 0) - 
            (Suffix.Count() > 0 && Message.Count() == 0 ? Rules.MainToSuffixJoin.Length : 0);

    public bool Fits(Snippet snippet) => 
        !MarkedComplete && ComposeText(Rules, Prefix, Message.Append(snippet), Suffix).Length <= Rules.MaxLength;

    public string ComposeText() => ComposeText(Rules, Prefix, Message, Suffix);

    public static string ComposeText(PostRenderRules rules, IEnumerable<Snippet> prefix, IEnumerable<Snippet> message, IEnumerable<Snippet> suffix)
    {
        return
            string.Join(rules.WordSpace, prefix.Select(s => s.Compose(rules))) +
            (prefix.Count() > 0 && message.Count() > 0 ? rules.PrefixToMainJoin : string.Empty) +
            string.Join(rules.WordSpace, message.Select(s => s.Compose(rules))) +
            (message.Count() > 0 && suffix.Count() > 0 ? rules.MainToSuffixJoin : string.Empty) +
            string.Join(rules.WordSpace, suffix.Select(s => s.Compose(rules)));
    }

    public IList<PostImage> Images { get; set; } = new List<PostImage>();

}
