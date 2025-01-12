using System.Collections;
using Presence.Posting.Lib.Connections.AT;
using Presence.Posting.Lib.Connections.Console;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public class ConnectionFactory
{
    public static async Task<INetworkConnection> CreateConnection(SocialNetwork network, IDictionary? env)
    {
        return network switch
        {
            SocialNetwork.Console => await ConnectTest(),
            SocialNetwork.AT => await ConnectAT(env),
            _ => throw new NotImplementedException($"Network {network} is not supported.")
        };
    }

    public static async Task<INetworkConnection> ConnectTest()
    {
        var connection = new ConsoleConnection();
        var ok = await connection.ConnectAsync(null);
        return ok ? connection : throw new Exception("Failed to connect to Test.");
    }

    public static async Task<INetworkConnection> ConnectAT(IDictionary? env)
    {
        var credentials = new ATCredentials(env);
        var connection = new ATConnection();
        var ok = await connection.ConnectAsync(credentials);
        return ok ? connection : throw new Exception("Failed to connect to AT network.");
    }
}