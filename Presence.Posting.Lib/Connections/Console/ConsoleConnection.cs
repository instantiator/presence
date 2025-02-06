using System.Diagnostics.CodeAnalysis;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections.Console;

public class ConsoleConnection : AbstractNetworkConnection
{
    [SetsRequiredMembers]
    public ConsoleConnection(ConsoleAccount account) : base(account)
    {
    }

    public override bool Connected => true;

    public override async Task<INetworkPostReference> PostAsync(CommonPost post, INetworkPostReference? replyTo = default)
    {
        var key = Guid.NewGuid().ToString();
        System.Console.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} ({key}): {(replyTo != null ? $"(reply to: {replyTo.NetworkReferences["guid"]}) " : "")}{post.ComposeText()}");
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
        System.Console.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} Deletion ({uri.NetworkReferences["guid"]})");
        return true;
    }

    protected override async Task<bool> ConnectImplementationAsync(INetworkAccount account)
    {
        System.Console.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} Connected: {account.AccountPrefix}");
        return true;
    }

    protected override async Task DisconnectImplementationAsync()
    {
        System.Console.WriteLine($"{Account[NetworkCredentialType.PrintPrefix]} Disconnect");
    }

}