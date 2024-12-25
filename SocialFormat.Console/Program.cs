using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using SocialFormat.Lib.Composition;
using SocialFormat.Lib.DTO;
using SocialFormat.Lib.Posts;

namespace SocialFormat.Console;

public class Program
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Path to input file containing a composition request.")]
        public string InputPath { get; set; } = null!;
    }

    public static void Main(string[] args)
    {
        Parser.Default
            .ParseArguments<Options>(args)
            .WithParsed(HandleOptions)
            .WithNotParsed(HandleErrors);
    }

    private static void HandleErrors(IEnumerable<Error> enumerable)
    {
        // Help text already shows
        // TODO: print extra info about errors
    }

    private static void HandleOptions(Options options)
    {
        var inputPath = options.InputPath;

        IEnumerable<CommonPost>? thread = null;
        Exception? exception = null;

        try
        {
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException($"Input file not found: {inputPath}", inputPath);
            }

            var input = File.ReadAllText(inputPath);
            var request = JsonSerializer.Deserialize<CompositionRequest>(input, opts)!;

            var simpleThreadRules = new ThreadCompositionRules
            {
                OnlyCountThreads = true,
                TagsOnAllPosts = true,
                TagsOnFirstPost = false,
                PostCounterPrefix = true,
                PostCounterSuffix = false
            };

            var simplePostRules = new PostRenderRules
            {
                MaxLength = 100,
                WordSpace = " ",
                PrefixToMainJoin = " ",
                MainToSuffixJoin = "\n",
                MinAcceptableSpace = 10,
                ShowLinkUrls = false,
                SplitSnippetTextOn = new[] { ' ', '\n' },
                TruncationMark = "…"
            };

            var composer = new SimpleThreadComposer(simpleThreadRules, simplePostRules);
            thread = composer.Compose(request).ToList();
        }
        catch (Exception e)
        {
            exception = e;
        }

        var result = new CompositionResponse
        {
            Thread = thread,
            Success = thread != null && exception == null,
            ExceptionType = exception?.GetType().ToString(),
            ExceptionMessage = exception?.Message,
            ExceptionStackTrace = exception?.StackTrace
        };
        System.Console.WriteLine(JsonSerializer.Serialize(result, opts));
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

}