namespace Presence.SocialFormat.Lib.Post;

public enum SnippetType
{
    Counter,
    Text,
    Link,
    Tag,
    Break,
    Image, // reserved
}

public class SocialSnippet
{
    public SocialSnippet()
    {
    }

    public SocialSnippet(string text, SnippetType snippetType = SnippetType.Text, string? reference = null, IEnumerable<SocialSnippetImage>? images = null)
    {
        Text = text;
        SnippetType = snippetType;
        Reference = reference;
        if (images != null) { Images.AddRange(images); }
    }

    public string? Text { get; init; } = null!;
    public SnippetType SnippetType { get; set; } = SnippetType.Text;
    public List<SocialSnippetImage> Images { get; set; } = new List<SocialSnippetImage>();
    public string? Reference { get; set; }

    public bool MayDivide => SnippetType == SnippetType.Text;
    public bool MayTruncate => SnippetType == SnippetType.Text;

    public Tuple<SocialSnippet?, SocialSnippet?> Divide(int space, PostRenderRules rules)
    {
        if (!MayDivide) { throw new Exception($"Cannot divide a {SnippetType} snippet"); }

        // easy case - if the snippet fits in the current post, add it
        if (string.IsNullOrWhiteSpace(Text) || Text.Length < space)
        {
            return new Tuple<SocialSnippet?, SocialSnippet?>(this, null);
        }

        var remainingWords = Text.Split(rules.SplitSnippetTextOn).ToList();

        if (remainingWords.Count() == 0)
        {
            return new Tuple<SocialSnippet?, SocialSnippet?>(this, null);
        }

        if (remainingWords.First().Length >= space)
        {
            return new Tuple<SocialSnippet?, SocialSnippet?>(null, this);
        }

        var firstWords = new List<string>();
        // while the next word will fit...
        while (string.Join(rules.WordSpace, firstWords.Append(remainingWords.First())).Length < space)
        {
            // pop the next word, and append to first words
            firstWords.Add(remainingWords.First());
            remainingWords.RemoveAt(0);
        }

        // images go on the first snippet
        var first = new SocialSnippet
        {
            SnippetType = SnippetType,
            Reference = Reference,
            Text = string.Join(rules.WordSpace, firstWords),
            Images = Images
        };

        var second = new SocialSnippet
        {
            SnippetType = SnippetType,
            Reference = Reference,
            Text = string.Join(rules.WordSpace, remainingWords)
        };

        // TODO: add a fallback option to split words if needed
        if (firstWords.Count == 0 && remainingWords.Count > 0)
        {
            throw new Exception($"Cannot divide this snippet - the first word '{remainingWords.First()}' of '{string.Join(rules.WordSpace, remainingWords)}' does not fit into space available: {space}");
        }

        return new Tuple<SocialSnippet?, SocialSnippet?>(first, second);
    }

    public SocialSnippet? Truncate(int space, PostRenderRules rules)
    {
        if (!MayTruncate) { throw new Exception($"Cannot truncate a {SnippetType} snippet"); }
        if (string.IsNullOrWhiteSpace(Text) || Text.Length <= space)
        {
            return this;
        }
        var remainingWords = Text.Split(rules.SplitSnippetTextOn).ToList();
        var fittedWords = new List<string>();

        while (remainingWords.Count > 0 && string.Join(rules.WordSpace, fittedWords.Append(remainingWords.First())).Length <= (space - rules.TruncationMark.Length))
        {
            // pop the next word, and append to first words
            fittedWords.Add(remainingWords.First());
            remainingWords.RemoveAt(0);
        }

        // TODO: add the option to truncate individual words if needed
        if (fittedWords.Count == 0 && remainingWords.Count > 0) 
        { 
            // throw new Exception($"Cannot truncate this snippet to {space} characters - the first word does not fit. Words: {string.Join(" ", remainingWords)}"); 
            return null;
        }

        return new SocialSnippet
        {
            SnippetType = SnippetType,
            Reference = Reference,
            Text = string.Join(rules.WordSpace, fittedWords) + rules.TruncationMark,
            Images = Images
        };
    }

    public string Compose(PostRenderRules rules)
    {
        var output = SnippetType switch
        {
            SnippetType.Text => Text,
            SnippetType.Link => rules.ShowLinkUrls ? Reference! : Text,
            SnippetType.Tag => ComposeTag(Text),
            SnippetType.Image => null,
            SnippetType.Counter => Text,
            SnippetType.Break => null,
            _ => throw new NotSupportedException($"Unsupported snippet type {SnippetType}")
        };

        return output ?? string.Empty;
    }

    public static string? ComposeTag(string? tag)
    {
        if (string.IsNullOrWhiteSpace(tag)) { return tag; }
        if (tag.StartsWith("#")) { return tag; }
        return "#" + tag;
    }
}