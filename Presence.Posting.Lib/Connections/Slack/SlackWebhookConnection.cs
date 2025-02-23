using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Corvus.Json;
using Presence.Posting.Lib.Connections.Slack.BlockKit;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Helpers;
using Presence.SocialFormat.Lib.Post;
using RestSharp;

namespace Presence.Posting.Lib.Connections.Slack;

public class SlackWebhookConnection : AbstractNetworkConnection
{
    private const int RATE_ms = 1000;
    private Uri? Webhook;

    [SetsRequiredMembers]
    public SlackWebhookConnection(INetworkAccount account) : base(account)
    {
    }

    public override bool Connected => Webhook != null && Webhook.UriIsHttp();

    [MemberNotNull(nameof(Webhook))]
    private void RequireAuthenticated()
    {
        if (!Connected || Webhook == null) throw new NullReferenceException("Webhook is not a valid URL");
    }

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null)
    {
        RequireAuthenticated();

        // TODO: upload local images (when implemented)
        var (text, blocks, notifications) = ComposeBlocks(post);

        var slackPost = new
        {
            text,
            blocks
        };

        var json = JsonSerializer.Serialize(slackPost);
        var request = new RestRequest().AddJsonBody(json);
        using var client = new RestClient(Webhook);
        
        await RateLimitAsync();
        var response = await client.PostAsync(request);

        if (!response.IsSuccessful)
        { 
            throw new Exception($"{response.ErrorMessage}, posting to Slack webhook: {Webhook}"); 
        }

        // TODO: search for and get the ts value for the slack post reference
        return new SlackWebhookPostReference(null, post);
    }

    protected (string, JsonAny[], IEnumerable<NetworkPostNotification>) ComposeBlocks(CommonPost post)
    {
        var text = post.ComposeText();
        var result = post.ToBlockKitJson();
        var blocks = result.Item1;
        var notifications = result.Item2;
        return (post.ComposeText(), blocks, notifications);
    }

    //     var section = new SectionBlock()
    //     {
    //         text = new RichTextBlock(post.ComposeText(), "mrkdwn"),
    //     };

    //     var sectionedBlocks = new List<MarkdownTextBlock>();
    //     var accessories = new List<SlackAccessoryBlock>();
    //     var notifications = new List<NetworkPostNotification>();
    //     var allItems = post.Compose().Select((pair, index) => new { pair, index });
    //     foreach (var item in allItems.Where(i => i != null && i.pair.Item1 != null && i.pair.Item2 != null))
    //     {
    //         var type = item.pair.Item1?.SnippetType;
    //         var reference = item.pair.Item1?.Reference;
    //         var text = item.pair.Item1?.Text;

    //         switch (type)
    //         {
    //             case SnippetType.Text:
    //                 text.RequireText("Text");
    //                 sectionedBlocks.Add(new MarkdownTextBlock(text!, "mrkdwn"));
    //                 break;
                
    //             case SnippetType.Counter:
    //                 // TODO: this is actually going to be some kind of context block
    //                 text.RequireText("Text");
    //                 sectionedBlocks.Add(new SectionBlock(text!));
    //                 break;

    //             case SnippetType.Tag:
    //                 // TODO: this is actually going to be some kind of context block
    //                 text.RequireText("Text");
    //                 sectionedBlocks.Add(new SectionBlock(text!));
    //                 break;

    //             case SnippetType.Link:
    //                 text.RequireText("Text");
    //                 reference.RequireText("Link");
    //                 sectionedBlocks.Add(new SlackBlock()
    //                 {
    //                     text = new SlackWebhookPostBlockText()
    //                     {
    //                         text = $"<{reference!}|{text!}>",
    //                         type = "mrkdwn"
    //                     },
    //                 });
    //                 break;

    //             case SnippetType.Break:
    //                 sectionedBlocks.Add(new SlackBlock()
    //                 {
    //                     type = "divider"
    //                 });
    //                 break;

    //             case SnippetType.Image:
    //                 text.RequireText("Image alt text");
    //                 reference.RequireText("Image url");
    //                 var refUrl = new Uri(reference!);
    //                 if (refUrl.UriIsHttp())
    //                 {
    //                     sectionedBlocks.Add(new SlackBlock()
    //                     {
    //                         accessory = new SlackWebhookPostBlockAccessory()
    //                         {
    //                             alt_text = text,
    //                             image_url = reference,
    //                             type = "image"
    //                         }
    //                     });
    //                 }
    //                 else
    //                 {
    //                     // TODO: generate warning - the image could not be included because no upload facility is available yet
    //                     notifications.Add(new NetworkPostNotification()
    //                     {
    //                         Message = $"Image upload to Slack not supported: {reference}",
    //                         NotificationType = NetworkPostNotificationType.ImageUpload,
    //                         Severity = NetworkPostNotificationSeverity.Warning
    //                     });
    //                 }
    //                 break;
    //             default:
    //                 throw new NotImplementedException($"{type} not supported yet.");
    //         }
    //     } // all items

    //     return (new SectionBlock()
    //     {

    //     }, notifications);
    // }

    protected override async Task<bool> ConnectImplementationAsync(INetworkAccount account)
    {
        Webhook = new Uri(account[NetworkCredentialType.IncomingWebhookUrl]!);
        return Connected;
    }

    public override Task<bool> DeletePostAsync(INetworkPostReference uri)
    {
        throw new NotImplementedException("Incoming webhooks do not support post deletion");
    }

    protected override async Task RateLimitImplementationAsync(DateTime? lastAction)
    {
        if (lastAction == null) { return; }
        var now = DateTime.Now;
        var elapsed = (TimeSpan)(now - lastAction!);
        if (elapsed.TotalMilliseconds < RATE_ms)
        {
            var delay = RATE_ms - elapsed.TotalMilliseconds;
            await Task.Delay((int)delay);
        }
    }

    protected override async Task DisconnectImplementationAsync()
    {
        Webhook = null;
    }
}