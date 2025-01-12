using Presence.SocialFormat.Lib.Networks;

namespace Presence.SocialFormat.Lib.Thread.Composition;

public class ThreadComposerIdentity
{
    public SocialNetwork Network { get; init; }
    public int Index { get; init; }
    public string Value => $"{Network}#{Index}";
    public override string ToString() => Value;
}