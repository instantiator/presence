using Presence.SocialFormat.Lib.Helpers;

namespace Presence.SocialFormat.Lib.Post;

public class CommonPost
{
    public CommonPost(int index, PostRenderRules rules)
    {
        Index = index;
        Rules = rules;
    }

    public int Index { get; set; }
    public PostRenderRules Rules { get; private set; }

    public IEnumerable<SocialSnippet> Prefix { get; set; } = new List<SocialSnippet>();
    public IEnumerable<SocialSnippet> Message { get; set; } = new List<SocialSnippet>();
    public IEnumerable<SocialSnippet> Suffix { get; set; } = new List<SocialSnippet>();
    public IList<CommonPostImage> Images { get; set; } = new List<CommonPostImage>();
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
        return string.Join(string.Empty, Compose(rules, prefix, message, suffix).Select(t => t.Item2));
    }

    public IEnumerable<Tuple<SocialSnippet?, string>> Compose() => Compose(Rules, Prefix, Message, Suffix);

    public static IEnumerable<Tuple<SocialSnippet?, string>> Compose(PostRenderRules rules, IEnumerable<SocialSnippet> prefix, IEnumerable<SocialSnippet> message, IEnumerable<SocialSnippet> suffix)
    {
        var prefixInterspersed = prefix
            .Select(s => new Tuple<SocialSnippet?, string>(s, s.Compose(rules)))
            .Intersperse(new Tuple<SocialSnippet?, string>(null, rules.WordSpace));

        var messageInterspersed = message
            .Select(s => new Tuple<SocialSnippet?, string>(s, s.Compose(rules)))
            .Intersperse(new Tuple<SocialSnippet?, string>(null, rules.WordSpace));

        var suffixInterspersed = suffix
            .Select(s => new Tuple<SocialSnippet?, string>(s, s.Compose(rules)))
            .Intersperse(new Tuple<SocialSnippet?, string>(null, rules.WordSpace));

        var all = new List<Tuple<SocialSnippet?, string>>()
            .Concat(prefixInterspersed)
            .Append(new Tuple<SocialSnippet?, string>(null, prefix.Count() > 0 && message.Count() > 0 ? rules.PrefixToMainJoin : string.Empty))
            .Concat(messageInterspersed)
            .Append(new Tuple<SocialSnippet?, string>(null, message.Count() > 0 && suffix.Count() > 0 ? rules.MainToSuffixJoin : string.Empty))
            .Concat(suffixInterspersed);

        return all;
    }

    public static CommonPost ImageOverflowPost(int index, PostRenderRules rules, IEnumerable<CommonPostImage> images, string text, IEnumerable<SocialSnippet> prefix, IEnumerable<SocialSnippet> suffix)
    {
        return new CommonPost(index, rules)
        {
            Prefix = prefix.ToList(),
            Message = [new SocialSnippet(text)],
            Suffix = suffix.ToList(),
            Images = images.ToList()
        };
    }
}
