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
        if (!Connected || Webhook == null) throw new InvalidOperationException("Not connected: call ConnectAsync first");
    }

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = null)
    {
        RequireAuthenticated();

        // Local images can't be uploaded yet (see #35): ComposeBlocks reports them as notifications
        var (blocks, notifications) = ComposeBlocks(post);

        var slackPost = new SlackWebhookPost
        {
            Text = post.ComposeText(),
            Blocks = blocks
        };

        var json = JsonSerializer.Serialize(slackPost);
        var request = new RestRequest().AddJsonBody(json);
        using var client = new RestClient(Webhook);

        await RateLimitAsync();
        var response = await client.PostAsync(request);

        if (!response.IsSuccessful)
        {
            throw new HttpRequestException($"{response.ErrorMessage}, posting to Slack webhook: {Webhook}", response.ErrorException, response.StatusCode);
        }

        // Incoming webhooks don't return the post's ts, so the reference can't identify it (see #37)
        return new SlackWebhookPostReference(null, post);
    }

    protected static (IEnumerable<SlackWebhookPostBlock>, IEnumerable<NetworkPostNotification>) ComposeBlocks(CommonPost post)
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
                        Text = new SlackWebhookPostBlockText()
                        {
                            Text = text,
                            Type = "mrkdwn" // support basic formatting if used
                        },
                    });
                    break;

                case SnippetType.Link:
                    text.RequireText("Text");
                    reference.RequireText("Link");
                    blocks.Add(new SlackWebhookPostBlock()
                    {
                        Text = new SlackWebhookPostBlockText()
                        {
                            Text = $"<{reference}|{text}>",
                            Type = "mrkdwn"
                        },
                    });
                    break;

                case SnippetType.Break:
                    blocks.Add(new SlackWebhookPostBlock()
                    {
                        Type = "divider"
                    });
                    break;

                case SnippetType.Image:
                    text.RequireText("Image alt text");
                    reference.RequireText("Image url");
                    var refUrl = new Uri(reference);
                    if (refUrl.UriIsHttp())
                    {
                        blocks.Add(new SlackWebhookPostBlock()
                        {
                            Accessory = new SlackWebhookPostBlockAccessory()
                            {
                                AltText = text,
                                ImageUrl = reference,
                                Type = "image"
                            }
                        });
                    }
                    else
                    {
                        notifications.Add(new NetworkPostNotification()
                        {
                            Message = $"Image upload to Slack not supported: {reference}",
                            NotificationType = NetworkPostNotificationType.ImageUpload,
                            Severity = NetworkPostNotificationSeverity.Warning
                        });
                    }
                    break;
                default:
                    throw new NotSupportedException($"{type} not supported yet.");
            }
        } // all items
        return (blocks, notifications);
    }

    protected override async Task<bool> ConnectImplementationAsync(INetworkAccount account)
    {
        Webhook = account.TryGetValue(NetworkCredentialType.IncomingWebhookUrl, out var url) && Uri.TryCreate(url, UriKind.Absolute, out var webhook)
            ? webhook
            : throw new ArgumentException($"Missing or invalid credential: {NetworkCredentialType.IncomingWebhookUrl}", nameof(account));
        return Connected;
    }

    public override Task<bool> DeletePostAsync(INetworkPostReference reference)
    {
        throw new NotSupportedException("Incoming webhooks do not support post deletion");
    }

    protected override async Task RateLimitImplementationAsync(DateTime? lastAction)
    {
        if (lastAction == null) { return; }
        var now = DateTime.Now;
        var elapsed = now - lastAction.Value;
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