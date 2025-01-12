using CommandLine;
using Presence.SocialFormat.Lib.Constants;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.IO;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Thread.Builder;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Console;

public class Program
{
    public class Options
    {
        [Option('f', "input-file", Required = false, HelpText = "Path to input file containing a composition request.")]
        public string? InputPath { get; set; } = null;

        [Option('n', "networks", Required = true, HelpText = "Social networks to generate for, a comma-separated list.", Separator = ',', Min = 1)]
        public IEnumerable<SocialNetwork> Network { get; set; } = null!;

        [Option('i', "input-format", Required = false, Default = InputFormat.Json, HelpText = $"Set the input format.")]
        public InputFormat InputFormat { get; set; } = InputFormat.Json;

        [Option('o', "output-format", Required = false, Default = OutputFormat.Json, HelpText = $"Set the output format.")]
        public OutputFormat OutputFormat { get; set; } = OutputFormat.Json;
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
        System.Console.Error.WriteLine("  Input formats:   " + Helpers.EnumAsCSV(typeof(InputFormat)));
        System.Console.Error.WriteLine("  Output formats:  " + Helpers.EnumAsCSV(typeof(OutputFormat)));
        System.Console.Error.WriteLine("  Social networks: " + Helpers.EnumAsCSV(typeof(SocialNetwork)));
    }

    private static void HandleOptions(Options options)
    {
        try
        {
            var request = InputReader.Decode(options.InputFormat, options.InputPath);
            var composers = options.Network.Select(ComposerFactory.ForNetwork);
            var response = new ThreadBuilder(composers).WithRequest(request).Build();
            System.Console.WriteLine(OutputWriter.Encode(options.OutputFormat, response));
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
        }
    }
}