using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO.Text;

public class MarkdownFormatWriter : IFormatWriter
{
    public string ToString(ThreadCompositionResponse response)
    {
        var lines = new List<string>();
        foreach (var thread in response.Threads)
        {
            lines.Add($"## {thread.Value.Identity.Value}");
            lines.Add(string.Empty);

            foreach (var post in thread.Value.Posts)
            {
                lines.Add($"{post.ComposeText()}");
                lines.Add(string.Empty);

                if (post.Images.Count() > 0)
                {
                    lines.Add("| " + string.Join(" | ", new string[post.Images.Count()].Select((_, i) => $"Image {i + 1}")) + " |");
                    lines.Add("|" + string.Join("|", new string[post.Images.Count()].Select((_, i) => $"-")) + "|");
                    lines.Add("|" + string.Join(" | ", post.Images.Select(img => $"![{img.Text}]({img.Reference})")) + " |");
                    lines.Add(string.Empty);
                }
            }

            if (thread.Value.ExceptionType != null)
            {
                lines.Add($"❌ {thread.Value.ExceptionType}: {thread.Value.ExceptionMessage}");
                lines.Add(string.Empty);
            }
        }

        return string.Join("\n", lines);
    }
}