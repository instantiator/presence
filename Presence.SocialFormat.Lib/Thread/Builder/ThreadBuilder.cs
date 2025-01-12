using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.Thread.Builder;

public class ThreadBuilder
{
    public List<IThreadComposer> Composers { get; set; } = new List<IThreadComposer>();
    public List<SocialSnippet> Message { get; set; } = new List<SocialSnippet>();
    public List<SocialSnippet> Tags { get; set; } = new List<SocialSnippet>();

    public ThreadBuilder()
    {
    }

    public ThreadBuilder(SocialNetwork network)
    {
        Composers.Add(ComposerFactory.ForNetwork(network));
    }

    public ThreadBuilder(IThreadComposer composer)
    {
        Composers.Add(composer);
    }

    public ThreadBuilder(IEnumerable<IThreadComposer> composers)
    {
        Composers.AddRange(composers);
    }

    public ThreadBuilder(SocialNetwork network, ThreadCompositionRules threadRules, PostRenderRules postRules, ICounterCreator? counterCreator = null)
    {
        Composers.Add(new SimpleThreadComposer(network, threadRules, postRules, counterCreator));
    }

    public ThreadBuilder WithComposer(SocialNetwork network)
    {
        Composers.Add(ComposerFactory.ForNetwork(network));
        return this;
    }

    public ThreadBuilder WithComposer(IThreadComposer composer)
    {
        Composers.Add(composer);
        return this;
    }

    public ThreadBuilder WithComposer(SocialNetwork network, ThreadCompositionRules threadRules, PostRenderRules postRules, ICounterCreator? counterCreator = null)
    {
        Composers.Add(new SimpleThreadComposer(network, threadRules, postRules, counterCreator));
        return this;
    }

    public ThreadBuilder WithComposers(IEnumerable<IThreadComposer> composers)
    {
        Composers.AddRange(composers);
        return this;
    }

    public ThreadBuilder WithMessage(SocialSnippet snippet)
    {
        Message.Add(snippet);
        return this;
    }

    public ThreadBuilder WithMessages(IEnumerable<SocialSnippet> snippets)
    {
        Message.AddRange(snippets);
        return this;
    }

    public ThreadBuilder WithTag(SocialSnippet snippet)
    {
        Tags.Add(snippet);
        return this;
    }

    public ThreadBuilder WithTags(IEnumerable<SocialSnippet> snippets)
    {
        Tags.AddRange(snippets);
        return this;
    }

    public ThreadBuilder WithRequest(ThreadCompositionRequest request)
    {
        Message = request.Message.ToList();
        Tags = request.Tags.ToList();
        return this;
    }

    public ThreadCompositionResponse Build()
    {
        if (Composers.Count == 0) { throw new ArgumentException("No composers specified.", "Composers"); }
        return new ThreadCompositionResponse
        {
            Threads = Composers.ToDictionary(
                composer => composer.Identity.Value,
                composer => composer.Compose(new ThreadCompositionRequest() { Message = Message, Tags = Tags }))
        };
    }

    public ThreadCompositionResponse Build(IEnumerable<IThreadComposer> composers)
    {
        if (composers.Count() == 0) { throw new ArgumentException("No composers specified.", "composers"); }
        return new ThreadCompositionResponse
        {
            Threads = composers.ToDictionary(
                composer => composer.Identity.Value,
                composer => composer.Compose(new ThreadCompositionRequest() { Message = Message, Tags = Tags }))
        };
    }

    public ThreadCompositionResponse Build(IThreadComposer composer)
    {
        return new ThreadCompositionResponse
        {
            Threads = new Dictionary<string, ComposedThread>()
            {
                {
                    composer.Identity.Value,
                    composer.Compose(new ThreadCompositionRequest() { Message = Message, Tags = Tags })
                }
            }
        };
    }
}