using System.Text.Json;
using CommandLine;
using DotEnv.Core;
using Presence.Posting.Lib.Connections;
using Presence.Posting.Lib.DTO;
using Presence.SocialFormat.Lib.IO;

namespace Presence.Posting.Console;

public class Program
{
    public class Options
    {
        [Option('f', "input-file", Required = false, HelpText = "Path to input file containing a thread composition response (leave blank to use stdin).")]
        public string? InputPath { get; set; } = null;

        [Option('e', "env-file", Required = false, HelpText = "Path to a file containing environment variables to use for connections.")]
        public string? EnvPath { get; set; } = null;
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
        var env = Environment.GetEnvironmentVariables();

        var summaries = new List<ThreadPostSummary>();
        var threadCompositionResponse = ThreadCompositionResponseInputReader.Decode(options.InputPath);
        foreach (var thread in threadCompositionResponse.Threads.Select(t => t.Value))
        {
            try
            {
                var connection = await ConnectionFactory.CreateConnection(thread.Identity.Network, env);
                var references = await connection.PostAsync(thread.Posts);
                var result = new ThreadPostSummary
                {
                    Identity = thread.Identity,
                    Thread = thread,
                    Success = true,
                    Reference = references.First()
                };
                summaries.Add(result);
            }
            catch (Exception e)
            {
                summaries.Add(new ThreadPostSummary
                {
                    Identity = thread.Identity,
                    Thread = thread,
                    Success = false,
                    Reference = null,
                    Exception = e
                });
            }
        } // each thread to post

        // summarise activity
        var response = new ThreadPostingResponse
        {
            Threads = summaries.ToDictionary(s => s.Identity.Value, s => s)
        };

        System.Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
        if (response.Threads.Any(s => !s.Value.Success))
        {
            foreach (var exception in response.Threads.Where(t => !t.Value.Success).Select(t => t.Value.Exception))
            {
                System.Console.WriteLine(exception?.ToString());
            }
        }
        return response;
    }

}