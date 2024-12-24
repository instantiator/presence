namespace SocialFormat.Lib.Posts;

public enum SnippetType
{
    Counter,
    Text,
    Link,
    HashTag,
    Break,
}

public class Snippet
{
    public string Text { get; set; } = null!;
    public SnippetType SnippetType { get; set; } = SnippetType.Text;
    public List<PostImage> Images { get; set; } = new List<PostImage>();
    public string? Reference { get; set; }
    public bool MayDivide => SnippetType == SnippetType.Text;
    public bool MayTruncate => SnippetType == SnippetType.Text || SnippetType == SnippetType.Link;

    public Tuple<Snippet?, Snippet?> Divide(int space, PostRenderRules rules)
    {
        if (!MayDivide) { throw new Exception($"Cannot divide a {SnippetType} snippet"); }

        // easy case - if the snippet fits in the current post, add it
        if (Text.Length < space)
        {
            return new Tuple<Snippet?, Snippet?>(this, null);
        }

        var remainingWords = Text.Split(rules.SplitSnippetTextOn).ToList();

        if (remainingWords.Count() == 0)
        {
            return new Tuple<Snippet?, Snippet?>(this, null);
        }

        if (remainingWords.First().Length >= space)
        {
            return new Tuple<Snippet?, Snippet?>(null, this);
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
        var first = new Snippet
        {
            SnippetType = SnippetType,
            Reference = Reference,
            Text = string.Join(rules.WordSpace, firstWords),
            Images = Images
        };

        var second = new Snippet
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

        return new Tuple<Snippet?, Snippet?>(first, second);
    }

    public Snippet Truncate(int space, PostRenderRules rules)
    {
        if (!MayTruncate) { throw new Exception($"Cannot truncate a {SnippetType} snippet"); }
        if (Text.Length <= space)
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

        // TODO: add the option truncated individual words if needed
        if (fittedWords.Count == 0 && remainingWords.Count > 0) { throw new Exception("Cannot truncate this snippet - the first word does not fit"); }

        return new Snippet
        {
            SnippetType = SnippetType,
            Reference = Reference,
            Text = string.Join(rules.WordSpace, fittedWords) + rules.TruncationMark,
            Images = Images
        };
    }

    public string Compose(PostRenderRules rules)
    {
        return SnippetType switch
        {
            SnippetType.Text => Text,
            SnippetType.Link => rules.ShowLinkUrls ? Reference! : Text,
            SnippetType.HashTag => Text,
            SnippetType.Counter => Text,
            SnippetType.Break => string.Empty,
            _ => throw new NotSupportedException($"Unsupported snippet type {SnippetType}")
        };
    }
}