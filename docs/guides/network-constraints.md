# Network constraints

Handling for the constraints described below are built into the library and tools. Under most circumstances, you won't need to worry about the character and image limits, or API rate limits. Content will be automatically split into threads. Posting will be rate limited to meet each network's requirements.

Network constraints may change at any time. The values below reflect knowledge encoded into Presence libraries and tools.

This information was last checked: `2025-02-17`

## Thread composition

For more information on each network's default configuration, see each `IThreadComposer` in the source code...

| `SocialNetwork` | `IThreadComposer`       | Character limit | Images (1st post) | Images (subsequently) | Notes       |
| --------------- | ----------------------- | --------------- | ----------------- | --------------------- | ----------- |
| `Console`       | `ConsoleThreadComposer` | `100`           | 4                 | 4                     | For testing |
| `AT`            | `ATThreadComposer`      | `300`           | 4                 | 4                     | eg. BlueSky |
| `Slack`         | `SlackThreadComposer`   | `40000`         | 4                 | 4                     |             |

## Thread posting

Each network's rate-limiting mechanism works differently. The quick summaries in the table below lose a lot of detail. See each network's documentation for full details.

| `SocialNetwork` | `IThreadComposer`       | API rate limit (quick summary)     | Documentation                                                                       |
| --------------- | ----------------------- | ---------------------------------- | ----------------------------------------------------------------------------------- |
| `Console`       | `ConsoleThreadComposer` | ∞                                  | N/A                                                                                 |
| `AT`            | `ATThreadComposer`      | 1666 creates/hr, 11666 creates/day | [Rate limits](https://docs.bsky.app/docs/advanced-guides/rate-limits) (BlueSky API) |
| `SlackWebhook`  | `SlackThreadComposer`   | 1 post/sec (short busts < 1/sec)   | [Rate limits](https://api.slack.com/apis/rate-limits) (Slack API)                   |
