using FinstarTestTask.Entities;

namespace FinstarTestTask.Helpers;

public static class FilteringHelper
{
    public static IQueryable<SomeObject> ApplyFilters(IQueryable<SomeObject> query, SomeObjectFilter filter)
    {
        if (filter.Number.HasValue)
        {
            query = query.Where(x => x.Number == filter.Number);
        }

        if (filter.Code.HasValue)
        {
            query = query.Where(x => x.Code == filter.Code);
        }

        if (!string.IsNullOrEmpty(filter.Value))
        {
            query = query.Where(x => x.Value.Contains(filter.Value));
        }

        return query;
    }
}