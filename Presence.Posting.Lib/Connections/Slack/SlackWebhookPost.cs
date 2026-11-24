using System.Text.Json.Serialization;

namespace Presence.Posting.Lib.Connections.Slack;

public class SlackWebhookPost
{
    /// <summary>The post's text; Slack shows it when no blocks are sent.</summary>
    [JsonPropertyName("text")]
    public required string Text { get; set; }

    /// <summary>
    /// Block Kit layout for the post. Not sent yet: Slack currently receives the plain <see cref="Text"/> only,
    /// and moving to blocks changes how every post renders (see #36).
    /// </summary>
    [JsonIgnore]
    public IEnumerable<SlackWebhookPostBlock> Blocks { get; set; } = [];
}

public class SlackWebhookPostBlock
{
    /// <summary>The block type, eg. <c>section</c> or <c>divider</c>.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "section";

    [JsonPropertyName("block_id")]
    public string? BlockId { get; set; }

    /// <summary>The block's text.</summary>
    [JsonPropertyName("text")]
    public SlackWebhookPostBlockText? Text { get; set; }

    /// <summary>An element shown alongside the block's text, eg. an image.</summary>
    [JsonPropertyName("accessory")]
    public SlackWebhookPostBlockAccessory? Accessory { get; set; }
}

public class SlackWebhookPostBlockText
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }

    /// <summary>The text format: <c>mrkdwn</c> or <c>plain_text</c>.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "mrkdwn";
}

public class SlackWebhookPostBlockAccessory
{
    /// <summary>The element type, eg. <c>image</c>.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "image";

    /// <summary>The image's URL.</summary>
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    /// <summary>The image's alt text.</summary>
    [JsonPropertyName("alt_text")]
    public string? AltText { get; set; }
}
