using Presence.SocialFormat.Lib.Composition;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Builder;

public class ThreadBuilder
{
    public List<IThreadComposer> Composers { get; set; } = new List<IThreadComposer>();
    public List<SocialSnippet> Message { get; set; } = new List<SocialSnippet>();
    public List<SocialSnippet> Tags { get; set; } = new List<SocialSnippet>();

    public ThreadBuilder()
    {
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

    public IDictionary<IThreadComposer, IEnumerable<CommonPost>> Build()
    {
        if (Composers.Count == 0) { throw new ArgumentException("No composers specified.", "Composers"); }
        return Composers.ToDictionary(composer => composer, Build);
    }

    public IDictionary<IThreadComposer, IEnumerable<CommonPost>> Build(IEnumerable<IThreadComposer> composers)
    {
        if (composers.Count() == 0) { throw new ArgumentException("No composers specified.", "composers"); }
        return composers.ToDictionary(composer => composer, Build);
    }

    public IEnumerable<CommonPost> Build(IThreadComposer composer)
    {
        return composer.Compose(new CompositionRequest() { Message = Message, Tags = Tags });
    }
}