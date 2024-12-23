using SocialFormat.Lib.Posts;

namespace SocialFormat.Lib.Composition;

public interface IThreadComposer
{
    PostRenderRules PostRules { get; }
    ThreadCompositionRules ThreadRules { get; }
    IEnumerable<CommonPost> Compose(IEnumerable<SocialSnippet> snippets, IEnumerable<SocialSnippet> tags);
}