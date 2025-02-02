using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.Com.Atproto.Repo;
using FishyFlip.Models;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.Posting.Lib.Connections;

public class ATPostReference : INetworkPostReference
{
    public ATPostReference(CreateRecordOutput output, CommonPost? origin = null, Post? atOrigin = null)
    {
        this.Output = output;
        this.Origin = origin;
        this.AtOrigin = atOrigin;
    }

    public CreateRecordOutput Output { get; private set; }
    public ATUri Uri => Output.Uri!;
    public ATDid Did => Uri.Did!;
    public string Cid => Output.Cid!;
    public string RKey => Uri.Rkey;
    public CommonPost? Origin { get; private set; }
    public Post? AtOrigin { get; private set; }
    public IDictionary<string, string> NetworkReferences => new Dictionary<string, string>
    {
        { "uri", Uri.ToString() },
        { "did", Did.ToString() },
        { "cid", Cid },
        { "rkey", RKey }
    };

    public SocialNetwork Network => SocialNetwork.AT;

}