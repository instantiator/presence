namespace Presence.SocialFormat.Console;

internal static class Helpers
{
    public static string EnumAsCSV(Type t) => string.Join(", ", Enum.GetNames(t));
}