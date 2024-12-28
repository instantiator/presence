using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using Presence.SocialFormat.Lib.Composition;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Console;

public class Program
{
    public class Options
    {
        [Option('i', "input-file", Required = true, HelpText = "Path to input file containing a composition request.")]
        public string InputPath { get; set; } = null!;

        [Option('n', "networks", Required = true, HelpText = "Social networks to generate for, a comma-separated list.", Separator = ',', Min = 1)]
        public IEnumerable<SocialNetwork> Network { get; set; } = null!;

        [Option('o', "output-format", Required = false, Default = OutputFormat.Json, HelpText = $"Set the output format.")]
        public OutputFormat Format { get; set; } = OutputFormat.Json;
    }

    public static void Main(string[] args)
    {
        var parser = new Parser((with) =>
        {
            with.AutoHelp = true;
            with.AutoVersion = true;
            with.CaseSensitive = false;
            with.HelpWriter = System.Console.Error;
        });

        parser
            .ParseArguments<Options>(args)
            .WithParsed(HandleOptions)
            .WithNotParsed(HandleErrors);
    }

    private static void HandleErrors(IEnumerable<Error> errors)
    {
        System.Console.Error.WriteLine("Supported constants:");
        System.Console.Error.WriteLine("  Output formats:  " + Helpers.EnumAsCSV(typeof(OutputFormat)));
        System.Console.Error.WriteLine("  Social networks: " + Helpers.EnumAsCSV(typeof(SocialNetwork)));
    }

    private static void HandleOptions(Options options)
    {
        var inputPath = options.InputPath;

        IDictionary<SocialNetwork, IEnumerable<CommonPost>>? threads = null;
        Exception? exception = null;

        try
        {
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException($"Input file not found: {inputPath}", inputPath);
            }

            var input = File.ReadAllText(inputPath);
            var request = JsonSerializer.Deserialize<CompositionRequest>(input, opts)!;

            var composers = options.Network.Select(ComposerFactory.ForNetwork);
            threads = composers.ToDictionary(composer => composer.Network, composer => composer.Compose(request));
        }
        catch (Exception e)
        {
            exception = e;
        }

        var result = new CompositionResponse
        {
            Threads = threads,
            Success = threads != null && threads.Count() == options.Network.Count() && exception == null,
            ExceptionType = exception?.GetType().ToString(),
            ExceptionMessage = exception?.Message,
            ExceptionStackTrace = exception?.StackTrace
        };

        switch (options.Format)
        {
            case OutputFormat.Json:
                System.Console.WriteLine(JsonSerializer.Serialize(result, opts));
                break;
            case OutputFormat.HumanReadable:
                System.Console.WriteLine(HumanReadable(result));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(options.Format), options.Format, "Unknown output format.");
        }
    }

    // strict on unknown properties, relaxed on case sensitivity
    private static JsonSerializerOptions opts = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
        WriteIndented = true
    };

    private static string HumanReadable(CompositionResponse response)
    {
        var separator = "---";
        var lines = new List<string>();

        if (response.Success)
        {
            foreach (var network in response.Threads!.Keys)
            {
                lines.Add($"Network: {network}");
                lines.Add(string.Join($"\n  {separator}\n", response.Threads![network].Select(p => "✉️ " + p.ComposeText())));
            }
        }
        else
        {
            lines.Add("Exception encountered processing thread.");
            lines.Add(separator);
            lines.Add($"Exception: {response.ExceptionType}");
            lines.Add($"Message: {response.ExceptionMessage}");
            lines.Add($"Stack trace: {response.ExceptionStackTrace}");
        }

        return string.Join("\n", lines);
    }
}