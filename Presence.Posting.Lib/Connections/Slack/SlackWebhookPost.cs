using System.Diagnostics.CodeAnalysis;

namespace Presence.Posting.Lib.Connections.Slack;

public class SlackPostBlock
{
    public required string text { get; init; } = null!;

    public required IEnumerable<SlackBlock> blocks { get; init; } = [];
}

public class SectionBlock : SlackBlock
{
    [SetsRequiredMembers]
    public SectionBlock()
    {
        this.type = "section";
    }

    [SetsRequiredMembers]
    public SectionBlock(string text) : this(new MarkdownTextBlock(text, "mrkdwn")) {}

    [SetsRequiredMembers]
    public SectionBlock(SlackTextBlock block)
    {
        this.type = "section";
        this.text = block;
    }
}

public class MarkdownTextBlock : SlackTextBlock
{
    [SetsRequiredMembers]
    public MarkdownTextBlock(string text, string type)
    {
        this.type = type;
        this.text = text;
    }
}

public class SlackBlock
{
    public required string type { get; init; }
    public string? block_id { get; set; } = null;
    public SlackTextBlock? text { get; init; }
    public SlackAccessoryBlock? accessory { get; init; }
}

public class SlackTextBlock
{
    public required string text { get; init; }
    public required string type { get; init; }
}

public class SlackAccessoryBlock
{
    public SlackAccessoryBlock(string type)
    {
        this.type = type;
    }

    public required string type { get; init; }
    public string? image_url { get; set; } = null;
    public string? alt_text { get; set; } = null;
}

