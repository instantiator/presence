# Post to a social network

`Presence.SocialFormat.Posting` provides a `ConnectionFactory` class which can create and configure connections to each known social network.

Configuration for each network is required, and each will be different. For information about configuration for each network, see: [Network configuration specifics](./network-specifics.md)

```csharp
var env = new Dictionary<NetworkCredentialType, string>
{
    { NetworkCredentialType.AccountName, "your-account-name.bsky.social" },
    { NetworkCredentialType.AppPassword, "your-application-password" }
};

var connection = ConnectionFactory.CreateConnection("TEST1", SocialNetwork.AT, env);
await connection.ConnectAsync();
var post = new CommonPost(0, ATThreadComposer.AT_POST_RENDER_RULES)
{
    Message = [new SocialSnippet($"ATConnection_Posts: {DateTime.Now:O}")]
};
var result = await connection.PostAsync(post);
```

In the example above, configuration is provided in the `env` dictionary. It could also have come from environment variables. `ConnectionFactory` has a method called `CreateConnections` which will scan all environment variables, and generate a full list of connection objects.

```csharp
var env = Environment.GetEnvironmentVariables();
var connections = ConnectionFactory.CreateConnections(env);
```

Each object will need to be connected before it can be used:

```csharp
await connections.ElementAt(0).ConnectAsync();
```
