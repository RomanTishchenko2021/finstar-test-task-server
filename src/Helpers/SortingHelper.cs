using FinstarTestTask.Entities;

namespace FinstarTestTask.Helpers;

public static class SortingHelper
{
    public static IQueryable<SomeObject> ApplySorting(IQueryable<SomeObject> query, SomeObjectSort sort)
    {
        return sort.SortBy.ToLower() switch
        {
            "number" => sort.Descending ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number),
            "code" => sort.Descending ? query.OrderByDescending(x => x.Code) : query.OrderBy(x => x.Code),
            "value" => sort.Descending ? query.OrderByDescending(x => x.Value) : query.OrderBy(x => x.Value),
            _ => query
        };
    }
}