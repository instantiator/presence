using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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
        var (blocks, notifications) = ComposeBlocks(post);

        var slackPost = new SlackWebhookPost
        {
            text = post.ComposeText(),
            blocks = blocks
        };

        var json = JsonSerializer.Serialize(slackPost);
        var request = new RestRequest().AddJsonBody(json);
        using var client = new RestClient(Webhook);
        var response = await client.PostAsync(request);
        if (!response.IsSuccessful)
        { 
            throw new Exception($"{response.ErrorMessage}, posting to Slack webhook: {Webhook}"); 
        }

        // TODO: search for and get the ts value for the slack post reference
        return new SlackWebhookPostReference(null, post);
    }

    protected (IEnumerable<SlackWebhookPostBlock>, IEnumerable<NetworkPostNotification>) ComposeBlocks(CommonPost post)
    {
        var blocks = new List<SlackWebhookPostBlock>();
        var notifications = new List<NetworkPostNotification>();
        var allItems = post.Compose().Select((pair, index) => new { pair, index });
        foreach (var item in allItems.Where(i => i != null && i.pair.Item1 != null && i.pair.Item2 != null))
        {
            var type = item.pair.Item1?.SnippetType;
            var reference = item.pair.Item1?.Reference;
            var text = item.pair.Item1?.Text;

            switch (type)
            {
                case SnippetType.Text:
                case SnippetType.Counter:
                case SnippetType.Tag:
                    text.RequireText("Text");
                    blocks.Add(new SlackWebhookPostBlock()
                    {
                        text = new SlackWebhookPostBlockText()
                        {
                            text = text!,
                            type = "mrkdwn" // support basic formatting if used
                        },
                    });
                    break;

                case SnippetType.Link:
                    text.RequireText("Text");
                    reference.RequireText("Link");
                    blocks.Add(new SlackWebhookPostBlock()
                    {
                        text = new SlackWebhookPostBlockText()
                        {
                            text = $"<{reference!}|{text!}>",
                            type = "mrkdwn"
                        },
                    });
                    break;

                case SnippetType.Break:
                    blocks.Add(new SlackWebhookPostBlock()
                    {
                        type = "divider"
                    });
                    break;

                case SnippetType.Image:
                    text.RequireText("Image alt text");
                    reference.RequireText("Image url");
                    var refUrl = new Uri(reference!);
                    if (refUrl.UriIsHttp())
                    {
                        blocks.Add(new SlackWebhookPostBlock()
                        {
                            accessory = new SlackWebhookPostBlockAccessory()
                            {
                                alt_text = text,
                                image_url = reference,
                                type = "image"
                            }
                        });
                    }
                    else
                    {
                        // TODO: generate warning - the image could not be included because no upload facility is available yet
                        notifications.Add(new NetworkPostNotification()
                        {
                            Message = $"Image upload to Slack not supported: {reference}",
                            NotificationType = NetworkPostNotificationType.ImageUpload,
                            Severity = NetworkPostNotificationSeverity.Warning
                        });
                    }
                    break;
                default:
                    throw new NotImplementedException($"{type} not supported yet.");
            }
        } // all items
        return (blocks, notifications);
    }

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