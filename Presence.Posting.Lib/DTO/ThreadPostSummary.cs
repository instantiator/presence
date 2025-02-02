using System.Text.Json.Serialization;
using Presence.Posting.Lib.Connections;
using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Thread.Composition;

namespace Presence.Posting.Lib.DTO;

public class ThreadPostSummary
{
    public required ThreadComposerIdentity Identity { get; init; }
    [JsonIgnore] public ComposedThread? Thread { get; init; }
    public bool Success { get; init; }
    public INetworkPostReference? Reference { get; init; }
    [JsonIgnore] public Exception? Exception { get; init; }
    public string? ExceptionType => Exception?.GetType().Name;
    public string? ExceptionMessage => Exception?.Message;
}