namespace FinstarTestTask.Helpers;

public static class PaginationHelper
{
    public static Page<T> ToPagedList<T>(IQueryable<T> source, int pageNumber, int pageSize)
    {
        int totalCount = source.Count();
        List<T> items = source.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
        return new Page<T>(items, totalCount, pageNumber, pageSize);
    }
}