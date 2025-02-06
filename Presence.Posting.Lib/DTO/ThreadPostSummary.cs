using System.Text.Json.Serialization;
using Presence.Posting.Lib.Connections;
using Presence.SocialFormat.Lib.Networks;

namespace Presence.Posting.Lib.DTO;

public class ThreadPostSummary
{
    public required string AccountPrefix { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required SocialNetwork Network { get; init; }
    
    public bool Success { get; init; }

    public IEnumerable<IDictionary<string,string?>>? PostReferences { get; init; }
    
    public int? Posts { get; init; }
        
    [JsonIgnore] 
    public Exception? Exception { get; init; }
    
    public string? ExceptionType => Exception?.GetType().Name;
    
    public string? ExceptionMessage => Exception?.Message;
}