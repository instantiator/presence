namespace Presence.SocialFormat.Lib.Post;

public class CommonPostImage
{
    public string? SourceUrl { get; set; }
    public bool Uploaded { get; set; }
    public string AltText { get; set; } = null!;

    public static CommonPostImage From(SocialSnippet snippet)
    {
        if (snippet.SnippetType != SnippetType.Image)
        {
            throw new ArgumentException("Snippet must be an image snippet");
        }

        if (string.IsNullOrWhiteSpace(snippet.Text))
        {
            throw new ArgumentException("Image snippet must provide alt text");
        }

        return new CommonPostImage
        {
            SourceUrl = snippet.Reference,
            AltText = snippet.Text,
        };
    }

    public static CommonPostImage From(SocialSnippetImage snippetImage)
    {
        if (string.IsNullOrWhiteSpace(snippetImage.Alt))
        {
            throw new ArgumentException("Image must provide alt text");
        }

        return new CommonPostImage
        {
            SourceUrl = snippetImage.Src,
            AltText = snippetImage.Alt,
        };
    }
}
