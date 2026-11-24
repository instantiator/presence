using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.Com.Atproto.Repo;
using FishyFlip.Models;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public class ATPostReference : INetworkPostReference
{
    public ATPostReference(CreateRecordOutput output, string server, string handle, CommonPost? origin = null, Post? atOrigin = null, IEnumerable<NetworkPostNotification>? notifications = null)
    {
        Server = server;
        Handle = handle;
        Output = output;
        Uri = output.Uri ?? throw new ArgumentException("Post reference did not contain a Uri", nameof(output));
        Did = Uri.Did ?? throw new ArgumentException("Post reference Uri did not contain a Did", nameof(output));
        Cid = string.IsNullOrWhiteSpace(output.Cid) ? throw new ArgumentException("Post reference did not contain a Cid", nameof(output)) : output.Cid;
        Origin = origin;
        AtOrigin = atOrigin;
        Notifications = notifications ?? new List<NetworkPostNotification>();
    }

    public string Server { get; }
    public string Handle { get; }

    public CreateRecordOutput Output { get; private set; }
    public ATUri Uri { get; }
    public ATDid Did { get; }
    public string Cid { get; }
    public string RKey => Uri.Rkey;
    public CommonPost? Origin { get; private set; }
    public Post? AtOrigin { get; private set; }
    public IDictionary<string, string?> NetworkReferences => new Dictionary<string, string?>
    {
        { "uri", Uri.ToString() },
        { "did", Did.ToString() },
        { "cid", Cid },
        { "rkey", RKey },
        { "link", Link }
    };

    public SocialNetwork Network => SocialNetwork.AT;

    public string? Link => $"https://bsky.app/profile/{Handle}/post/{RKey}";

    public IEnumerable<NetworkPostNotification> Notifications { get; private set; }
}