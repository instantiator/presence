using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.Com.Atproto.Repo;
using FishyFlip.Models;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public class ATPostReference : INetworkPostReference
{
    public ATPostReference(CreateRecordOutput output, string server, string handle, CommonPost? origin = null, Post? atOrigin = null)
    {
        Server = server;
        Handle = handle;
        Output = output;
        Origin = origin;
        AtOrigin = atOrigin;
    }

    public string Server;
    public string Handle;

    public CreateRecordOutput Output { get; private set; }
    public ATUri Uri => Output.Uri!;
    public ATDid Did => Uri.Did!;
    public string Cid => Output.Cid!;
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
}