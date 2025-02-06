using System.Diagnostics.CodeAnalysis;

namespace Presence.SocialFormat.Lib.Helpers;

public class IEnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
    {
        if (x == null && y == null) { return true; }
        if (x == null || y == null) { return false; }
        return x.SequenceEqual(y);
    }

    public int GetHashCode([DisallowNull] IEnumerable<T> obj)
    {
        return obj.GetHashCode();
    }
}