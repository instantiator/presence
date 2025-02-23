using Corvus.Json;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.Slack.BlockKit;

public static class CommonPostExtensions
{
    public static (JsonAny[], IEnumerable<NetworkPostNotification>) ToBlockKitJson(this CommonPost post)
    {
        var notifications = new List<NetworkPostNotification>();

        var json = """{ type: "section", text: { type: "mrkdwn", text: "Hello, World!" } }""";
        var section = JsonAny.Parse(json);
        // var entity = Entity.FromItems(section);
        return ([ section ], notifications);
    }


}