using System.Text.RegularExpressions;
using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Helpers;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.IO.Text;

public class MarkdownFormatParser : IFormatParser
{
    public static Regex LinkRegex = new Regex($"^{RegexConstants.MD_LINK_REGEX}");
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
        if (string.IsNullOrWhiteSpace(str_trimmed))
        {
            return (null, null);
        }

        if (LinkRegex.IsMatch(str_trimmed)) // link or image
        {
            var match = LinkRegex.Match(str_trimmed);
            var text = match.Groups["text"].Value;
            var uriStr = match.Groups["uri"].Value;
            var uri = uriStr.ToUri();
            if (uri != null)
            {
                var metadata = uri.GetMetadata();
                if (metadata.Exists)
                {
                    return (new SocialSnippet
                    {
                        Text = text,
                        Reference = uriStr,
                        SnippetType = metadata.IsImage ? SnippetType.Image : SnippetType.Link,
                    },
                    str_trimmed.Substring(match.Index + match.Length).Trim());
                }
            }
            return (new SocialSnippet
            {
                Text = $"[{text}]({uriStr})",
                Reference = uriStr,
                SnippetType = SnippetType.Text,
            },
            str_trimmed.Substring(match.Index + match.Length).Trim());
        }
        else if (TagRegex.IsMatch(str_trimmed)) // tag
        {
            var match = TagRegex.Match(str_trimmed);
            var tag = match.Groups["tag"].Value;
            return (new SocialSnippet
            {
                Text = tag,
                SnippetType = SnippetType.Tag,
            },
            str_trimmed.Substring(match.Index + match.Length).Trim());
        }
        else // plain text
        {
            var word = str_trimmed.Split(" ").First();

            var wordAsUri = word.ToUri();
            if (wordAsUri != null && wordAsUri.GetMetadata().Exists)
            {
                return (new SocialSnippet
                {
                    Text = word,
                    Reference = word,
                    SnippetType = SnippetType.Link,
                },
                str_trimmed.Substring(word.Length).Trim());
            }
            else
            {
                return (new SocialSnippet
                {
                    Text = word,
                    SnippetType = SnippetType.Text,
                },
                str_trimmed.Substring(word.Length).Trim());
            }
        }
    }

    public string ToString(ThreadCompositionRequest request)
    {
        throw new NotImplementedException();
    }

    public string ToString(SocialSnippet snippet)
    {
        throw new NotImplementedException();
    }
}