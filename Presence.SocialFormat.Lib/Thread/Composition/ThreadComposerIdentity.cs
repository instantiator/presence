using Presence.SocialFormat.Lib.Networks;

namespace Presence.SocialFormat.Lib.Thread.Composition;

public class ThreadComposerIdentity
{
    public SocialNetwork Network { get; init; }
    public int Index { get; init; }
    public string Ident => $"{Network}#{Index}";
    public override string ToString() => Ident;
}