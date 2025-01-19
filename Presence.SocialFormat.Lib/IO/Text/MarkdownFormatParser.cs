using System.Text.RegularExpressions;
using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Helpers;
using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.IO.Text;

public class MarkdownFormatParser : IFormatParser
{
    public MarkdownFormatParser(ParserRules? rules = null)
    {
        this.rules = rules ?? ParserRules.Default;
    }

    private ParserRules rules { get; }

    public static Regex LinkRegex = new Regex($"^{RegexConstants.MD_LINK_REGEX}");
    public static Regex ImageRegex = new Regex($"^{RegexConstants.MD_IMAGE_REGEX}");
    public static Regex TagRegex = new Regex($"^{RegexConstants.TAG_REGEX}");

    public ThreadCompositionRequest ToRequest(string text)
    {
        var snippets = text
            .Split("\n")
            .Select(b => b.Trim())
            .SelectMany(b => ToSnippets(b));

        var message = snippets.Where(s => s.SnippetType != SnippetType.Tag);
        var tags = snippets.Where(s => s.SnippetType == SnippetType.Tag);

        return new ThreadCompositionRequest
        {
            Message = message,
            Tags = tags
        };
    }

    public IEnumerable<SocialSnippet> ToSnippets(string str)
    {
        // blank
        if (string.IsNullOrWhiteSpace(str))
        {
            return
            [
                new SocialSnippet
                {
                    SnippetType = SnippetType.Break
                }
            ];
        }

        var snippets = new List<SocialSnippet>();
        string? remainder = str;
        SocialSnippet? snippet;
        do
        {
            (snippet, remainder) = PullSnippet(remainder);
            if (snippet != null) { snippets.Add(snippet); }
        }
        while (snippet != null && !string.IsNullOrWhiteSpace(remainder));
        return snippets;
    }

    private (SocialSnippet?, string?) PullSnippet(string str)
    {
        var str_trimmed = str.Trim();
        if (string.IsNullOrWhiteSpace(str_trimmed)) { return (null, null); }
        else if (ImageRegex.IsMatch(str_trimmed)) { return GenImageSnippet(str_trimmed); }
        else if (LinkRegex.IsMatch(str_trimmed)) { return GenLinkSnippet(str_trimmed); }
        else if (TagRegex.IsMatch(str_trimmed)) { return GenTagSnippet(str_trimmed); }
        else { return GenTextOrLinkSnippet(str_trimmed); }
    }

    private (SocialSnippet?, string?) GenImageSnippet(string str)
    {
        var match = ImageRegex.Match(str);
        var text = match.Groups["text"].Value;
        var uriStr = match.Groups["uri"].Value;
        var uri = uriStr.ToUri();
        if (uri != null)
        {
            var metadata = uri.GetMetadata();
            if (rules.CheckLinks) {
                if (!metadata.Exists) { throw new Exception($"Link not found: {uri}"); }
            }
            return (new SocialSnippet
            {
                Text = text,
                Reference = uriStr,
                SnippetType = metadata.IsImage ? SnippetType.Image : SnippetType.Link,
            },
            str.Substring(match.Index + match.Length).Trim());
        }
        // if not a uri - treat as text
        return (new SocialSnippet
        {
            Text = $"[{text}]({uriStr})",
            Reference = uriStr,
            SnippetType = SnippetType.Text,
        },
        str.Substring(match.Index + match.Length).Trim());
    }

    private (SocialSnippet?, string?) GenLinkSnippet(string str)
    {
        var match = LinkRegex.Match(str);
        var text = match.Groups["text"].Value;
        var uriStr = match.Groups["uri"].Value;
        var uri = uriStr.ToUri();
        if (uri != null)
        {
            if (rules.CheckLinks) {
                var metadata = uri.GetMetadata();
                if (!metadata.Exists) { throw new Exception($"Link not found: {uri}"); }
            }
            return (new SocialSnippet
            {
                Text = text,
                Reference = uriStr,
                SnippetType = SnippetType.Link,
            },
            str.Substring(match.Index + match.Length).Trim());
        }
        // if not a uri - treat as text
        return (new SocialSnippet
        {
            Text = $"[{text}]({uriStr})",
            Reference = uriStr,
            SnippetType = SnippetType.Text,
        },
        str.Substring(match.Index + match.Length).Trim());
    }

    private (SocialSnippet?, string?) GenTagSnippet(string str)
    {
        var match = TagRegex.Match(str);
        var tag = match.Groups["tag"].Value;
        return (new SocialSnippet
        {
            Text = tag,
            SnippetType = SnippetType.Tag,
        },
        str.Substring(match.Index + match.Length).Trim());
    }

    private (SocialSnippet?, string?) GenTextOrLinkSnippet(string str)
    {
        var word = str.Split(" ").First();
        var wordAsUri = word.ToUri();
        if (wordAsUri != null && wordAsUri.GetMetadata().Exists)
        {
            return (new SocialSnippet
            {
                Text = word,
                Reference = word,
                SnippetType = SnippetType.Link,
            },
            str.Substring(word.Length).Trim());
        }
        else
        {
            return (new SocialSnippet
            {
                Text = word,
                SnippetType = SnippetType.Text,
            },
            str.Substring(word.Length).Trim());
        }
    }
}