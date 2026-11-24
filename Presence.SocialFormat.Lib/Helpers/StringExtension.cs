using System.Diagnostics.CodeAnalysis;

namespace Presence.SocialFormat.Lib.Helpers;

public static class StringExtension
{
    public static void RequireText([NotNull] this string? text, string? name = null)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException($"{name ?? "string"} cannot be empty or null.");
        }
    }
}