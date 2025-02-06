namespace Presence.Posting.Lib;

public static class LinqExtension
{
    public static IQueryable<TSource> If<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Func<IQueryable<TSource>, IQueryable<TSource>> branch)
    {
        return condition ? branch(source) : source;
    }

    public static IQueryable<TSource> If<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Func<IQueryable<TSource>, IQueryable<TSource>> onTrue,
        Func<IQueryable<TSource>, IQueryable<TSource>> onFalse)
    {
        return condition ? onTrue(source) : onFalse(source);
    }

}