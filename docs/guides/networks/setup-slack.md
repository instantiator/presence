# Set up a `Slack` network

## Incoming webhooks

You can create an incoming webhook for a simple Slack application. This is a url that a Slack application can use to post to Slack.

This process is a shortcut allowing you to circumvent a full OAuth flow. It's great for testing, and for simple cases (posting for yourself) it's much simpler than developing a fully blown application.

See: https://api.slack.com/messaging/webhooks

Steps:

1. Create a new app: https://api.slack.com/apps/new

- Set it up from scratch
- Give it a name
- Pick a workspace for it

2. Enable **Incoming webhooks**

- Select the **Incoming webhooks** section
- Enable the **Activate incoming webhooks** option

3. Add a webhook for your workspace

- Click **Add New Webhook to Workspace**
- Pick a channel the webhook can be used to post into
- Click **Allow**

There's now a new URL listed in the **Incoming webhooks** section, which you can copy and use as a part of your configuration.

> ⚠️ The webhook URL is a secret. Do not share it or add it to any open code repositories.

The Network part of each configuration key is always: `SlackWebhook`

## Network credentials

| Credential type      | Req      | Value                                                                                        |
| -------------------- | -------- | -------------------------------------------------------------------------------------------- |
| `IncomingWebhookUrl` | Required | The URL of the slack webhook belonging to the app, created for posting to a specific channel |

eg. The following line defines the `IncomingWebhookUrl` key and value in a `.env` configuration file:

```env
TEST2_SlackWebhook_IncomingWebhookUrl="https://hooks.slack.com/services/XXXXXXXXX/XXXXXXXXX/XXXXXXXXXXXXXXXXXXXXXXXX"
```

For examples, see: [Network specific configuration](../network-specifics.md)
