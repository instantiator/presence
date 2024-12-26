namespace SocialFormat.Console;

public static class Helpers
{
    public static string EnumAsCSV(Type t) => string.Join(", ", Enum.GetNames(t));
}