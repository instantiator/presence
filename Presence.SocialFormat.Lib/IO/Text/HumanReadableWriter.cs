using Presence.SocialFormat.Lib.DTO;

namespace Presence.SocialFormat.Lib.IO.Text;

public class HumanReadableWriter : IFormatWriter
{
    public string ToString(ThreadCompositionResponse response)
    {
        var separator = "---";
        var lines = new List<string>();

        foreach (var network in response.Threads!.Keys)
        {
            var thread = response.Threads![network];
            lines.Add($"Network: {network} {(thread.Success ? "✅" : "❌")}");
            lines.Add(separator);
            lines.Add(string.Join($"\n   {separator}\n", thread.Posts.Select(p => " ⏩ " + p.ComposeText())));
            if (thread.ExceptionType != null)
            {
                lines.Add($"   {separator}");
                lines.Add($" ❌ {thread.ExceptionType}: {thread.ExceptionMessage}");
            }
            lines.Add(""); // break
        }
        return string.Join("\n", lines);
    }
}