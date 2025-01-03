# Post to a social network

> ⚠️ This feature is under development.

`Presence.SocialFormat.Posting` provides a `ConnectionFactory` which can create and configure a connection to a specified `SocialNetwork`.

Configuration for each network is required, and each will be different. Each network connection uses configuration variables that start with a specific prefix. For instance, the `ATConnection` uses `AT_` by default.

```csharp
var env = new Dictionary<string, string>
{
    { "AT_AccountName", "your-account-name.bsky.social" },
    { "AT_AppPassword", "your-application-password" }
};

var connection = await ConnectionFactory.CreateConnection(SocialNetwork.AT, env);
var post = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES)
{
    Message = [new SocialSnippet($"ATConnection_Posts: {DateTime.Now:O}")]
};
var result = await connection.PostAsync(post);
```

In the example above, configuration is provided in the `env` dictionary. It could also have come from the shell environment...

```csharp
var env = Environment.GetEnvironmentVariables();
```

## AT/BlueSky specifics

* Create an application password in **Privacy & Security** settings in BlueSky.
* A connection to an AT network (eg. BlueSky) uses the following environment/configuration keys:
  * `AT_AccountName` (required) - your account name, eg. `instantiator.bsky.social`
  * `AT_AppPassword` (required) - an application password created in your account
  * `AT_Server` (optional) - the AT network server to connect to, if not `bsky.social`