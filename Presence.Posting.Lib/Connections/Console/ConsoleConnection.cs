using System.Diagnostics.CodeAnalysis;
using Presence.Posting.Lib.Constants;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.Console;

public class ConsoleConnection : AbstractNetworkConnection
{
    [SetsRequiredMembers]
    public ConsoleConnection(ConsoleAccount account) : base(account)
    {
    }

    private bool connected = false;
    public override bool Connected => connected;

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = default)
    {
        var key = Guid.NewGuid().ToString();
        System.Console.Error.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} ({key}): {(replyTo != null ? $"(reply to: {replyTo.NetworkReferences["guid"]}) " : "")}{post.ComposeText()}");
        return new ConsolePostReference
        {
            NetworkReferences = new Dictionary<string, string?>()
            {
                { "guid", key }
            },
            Origin = post
        };
    }

    public override async Task<bool> DeletePostAsync(INetworkPostReference uri)
    {
        System.Console.Error.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} Deletion ({uri.NetworkReferences["guid"]})");
        return true;
    }

    protected override async Task<bool> ConnectImplementationAsync(INetworkAccount account)
    {
        connected = true;
        System.Console.Error.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} Connected: {account.AccountPrefix}");
        return Connected;
    }

    protected override async Task DisconnectImplementationAsync()
    {
        connected = false;
        System.Console.Error.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} Disconnect");
    }

}