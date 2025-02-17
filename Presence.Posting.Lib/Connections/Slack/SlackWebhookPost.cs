namespace Presence.Posting.Lib.Connections.Slack;

public class SlackWebhookPost
{
    public string text { get; set; } = null!;

    public IEnumerable<SlackWebhookPostBlock> blocks = [];
}

public class SlackWebhookPostBlock
{
    public string type { get; set; } = "section"; // default
    public string? block_id { get; set; } = null;
    public SlackWebhookPostBlockText? text { get; set; } = null;
    public SlackWebhookPostBlockAccessory? accessory { get; set; } = null;
}

public class SlackWebhookPostBlockText
{
    public string text { get; set; } = null!;
    public string type { get; set; } = "mrkdwn"; // default
}

public class SlackWebhookPostBlockAccessory
{
    public string type { get; set; } = "image"; // default
    public string? image_url { get; set; } = null;
    public string? alt_text { get; set; } = null;
}

