using System.Text.Json;
using CommandLine;
using DotEnv.Core;
using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.Constants;
using Presence.Posting.Lib.DTO;
using Presence.SocialFormat.Lib.IO;

namespace Presence.Posting.Console;

public class Program
{
    public class Options
    {
        [Option('f', "input-file", Required = false, HelpText = "Path to input file containing a thread composition response (leave blank to use stdin).")]
        public string? InputPath { get; set; } = null;

        [Option('e', "env-file", Required = false, HelpText = "Path to a file containing environment variables to use for connections.", SetName = "config")]
        public string? EnvPath { get; set; } = null;

        [Option('j', "json-config", Required = false, HelpText = "JSON configuration string, defining accounts and account credentials.", SetName = "config")]
        public string? JsonConfig { get; set; } = null;
    }

    public static async Task Main(string[] args)
    {
        var parser = new Parser((with) =>
        {
            with.AutoHelp = true;
            with.AutoVersion = true;
            with.CaseSensitive = false;
            with.HelpWriter = System.Console.Error;
        });

        var outcome = await parser
            .ParseArguments<Options>(args)
            .WithParsedAsync(HandleOptions);
    }

    private static async Task<ThreadPostingResponse> HandleOptions(Options options)
    {
        if (options.EnvPath != null)
        {
            new EnvLoader().AddEnvFile(options.EnvPath).Load();
        }
        if (options.JsonConfig != null)
        {
            Environment.SetEnvironmentVariable(ConfigKeys.JSON_DATA_ENV_KEY, options.JsonConfig);
        }
        var env = Environment.GetEnvironmentVariables();
        var connections = ConnectionFactory.CreateConnections(env);
        var summaries = new List<ThreadPostSummary>();
        var threadCompositionResponse = ThreadCompositionResponseInputReader.Decode(options.InputPath);

        foreach (var connection in connections)
        {
            try
            {
                var threads = threadCompositionResponse.Threads.Where(t => t.Key == connection.Network);
                if (threads.Count() != 1) { throw new KeyNotFoundException($"{threads.Count()} threads found for network {connection.Network}"); }
                var thread = threads.Single().Value;
                await connection.ConnectAsync();
                var references = await connection.PostAsync(thread.Posts);
                var result = new ThreadPostSummary
                {
                    AccountPrefix = connection.Prefix,
                    Network = connection.Network,
                    PostReferences = references.Select(r => r.NetworkReferences),
                    Posts = references?.Count(),
                    Success = true
                };
                summaries.Add(result);
            }
            catch (Exception e)
            {
                summaries.Add(new ThreadPostSummary
                {
                    AccountPrefix = connection.Prefix,
                    Network = connection.Network,
                    Success = false,
                    Exception = e
                });
            }
        } // each thread to post

        // summarise activity
        var response = new ThreadPostingResponse
        {
            Summaries = summaries
                .Select(s => s.AccountPrefix)
                .ToDictionary(
                    p => p, 
                    p => summaries.Where(s => s.AccountPrefix == p).ToDictionary(s => s.Network, s => s))
        };

        System.Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
        return response;
    }

}