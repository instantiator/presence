using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.Posting.Lib.Connections;

public class ATPostReference : INetworkPostReference
{
    public ATPostReference(ATUri uri, CommonPost? origin = null, Post? atOrigin = null)
    {
        this.Uri = uri;
        this.Origin = origin;
        this.AtOrigin = atOrigin;
    }

    public ATUri Uri { get; private set; }
    public CommonPost? Origin { get; private set; }
    public Post? AtOrigin { get; private set; }
    public string ReferenceKey => this.Uri.Rkey;
    public SocialNetwork Network => SocialNetwork.AT;

}