using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.DTO;

public class ComposedThread
{
    public ComposedThread(ThreadComposerIdentity identity, IEnumerable<CommonPost> posts, bool success, Exception? e)
    {
        Identity = identity;
        Posts = posts;
        Success = success;
        exception = e;
    }

    public ThreadComposerIdentity Identity { get; init; }
    public IEnumerable<CommonPost> Posts { get; init; }
    public bool Success { get; init; }
    private Exception? exception = null;
    public string? ExceptionType => exception?.GetType().Name;
    public string? ExceptionMessage => exception?.Message;
}