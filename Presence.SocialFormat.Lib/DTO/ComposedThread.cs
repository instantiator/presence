using System.Diagnostics.CodeAnalysis;
using Presence.SocialFormat.Lib.Post;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.SocialFormat.Lib.DTO;

public class ComposedThread
{
    public ComposedThread()
    {
    }

    [SetsRequiredMembers]
    public ComposedThread(ThreadComposerIdentity identity, IEnumerable<CommonPost> posts, bool success, Exception? e)
    {
        Identity = identity;
        Posts = posts;
        Success = success;
        exception = e;
    }

    public required ThreadComposerIdentity Identity { get; init; }
    public required IEnumerable<CommonPost> Posts { get; init; }
    public required bool Success { get; init; }
    private Exception? exception = null;
    public string? ExceptionType => exception?.GetType().Name;
    public string? ExceptionMessage => exception?.Message;
}