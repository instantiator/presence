using System.Collections;
using Presence.Posting.Lib.Connections.AT;
using Presence.Posting.Lib.Connections.Console;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.Connections;

public class ConnectionFactory
{
    public static IEnumerable<INetworkConnection> CreateConnections(IDictionary env)
    {
        var environment = new EnvironmentConfigReader(env);
        return environment.Keys.SelectMany((prefix) => environment[prefix].Keys.Select((network) => CreateConnection(prefix, network, environment[prefix][network])));
    }

    public static INetworkConnection CreateConnection(string prefix, SocialNetwork network, IDictionary<NetworkCredentialType, string> credentials)
    {
        return network switch
        {
            SocialNetwork.Console => CreateConsole(prefix, credentials),
            SocialNetwork.AT => CreateAT(prefix, credentials),
            _ => throw new NotImplementedException($"Network {network} is not supported.")
        };
    }

    public static ConsoleConnection CreateConsole(string prefix, IDictionary<NetworkCredentialType, string> credentials)
        => new ConsoleConnection(new ConsoleAccount(prefix, credentials));

    public static ATConnection CreateAT(string prefix, IDictionary<NetworkCredentialType, string> credentials)
        => new ATConnection(new ATAccount(prefix, credentials));
}