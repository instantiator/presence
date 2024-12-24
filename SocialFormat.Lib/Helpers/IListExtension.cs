namespace SocialFormat.Lib.Helpers;

public static class IListExtension
{
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        if (items == null) throw new ArgumentNullException(nameof(items));

        if (list is List<T> asList)
        {
            asList.AddRange(items);
        }
        else
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}