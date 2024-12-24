using SocialFormat.Lib.Posts;

namespace SocialFormat.Lib.Composition;

public interface IThreadComposer
{
    PostRenderRules PostRules { get; }
    ThreadCompositionRules ThreadRules { get; }
    IEnumerable<Post> Compose(IEnumerable<Snippet> snippets, IEnumerable<Snippet> tags);
}