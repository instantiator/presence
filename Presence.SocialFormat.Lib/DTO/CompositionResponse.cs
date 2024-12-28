using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.DTO;

public class CompositionResponse
{
    public required bool Success { get; init; }
    public required IDictionary<SocialNetwork, IEnumerable<CommonPost>>? Threads { get; init; }

    public string? ExceptionType { get; init; }
    public string? ExceptionMessage { get; init; }
    public string? ExceptionStackTrace { get; init; }
}