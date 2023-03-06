using System.Linq.Dynamic.Core;
using System.Text;

namespace Core.Application.DynamicQuery;

// visit https://dynamic-linq.net/overview for more information about dynamic-linq
public static class IQueryableDynamicFilterExtensions
{
    // Declaring filter operators. You can add extra operators when you needed.
    private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
    {
        { "eq", "=" },
        { "neq", "!=" },
        { "lt", "<" },
        { "lte", "<=" },
        { "gt", ">" },
        { "gte", ">=" },
        { "isnull", "== null" },
        { "isnotnull", "!= null" },
        { "startswith", "StartsWith" },
        { "endswith", "EndsWith" },
        { "contains", "Contains" },
        { "doesnotcontain", "Contains" }
    };

    // This method is using for apply filtering and sorting to the query.
    public static IQueryable<TEntity> ToDynamic<TEntity>(this IQueryable<TEntity> query, Dynamic dynamic)
    {
        if (dynamic.Filter is not null)
            query = Filter(query, dynamic.Filter);
        if (dynamic.Sort is not null && dynamic.Sort.Any())
            query = Sort(query, dynamic.Sort);
        if (dynamic.Select is not null)
            query = Select(query, dynamic.Select);
        return query;
    }

    private static IQueryable<TEntity> Select<TEntity>(IQueryable<TEntity> queryable, string select)
       => queryable.Select<TEntity>($"new {{ {select} }}");

    // Apply Filter
    private static IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> queryable, Filter filter)
    {
        // Getting all filters into one list.
        IList<Filter> filters = GetAllFilters(filter);
        // Getting filter values for use in Where method.
        string?[] values = filters.Select(f => f.Value).ToArray();
        // Getting predicate string for use in where method.
        string predicate = Transform(filter, filters);
        // Filter is implemented with the help of the System.Linq.Dynamic.Core package
        queryable = queryable.Where(predicate, values);

        return queryable;
    }

    // Apply Sorting
    private static IQueryable<TEntity> Sort<TEntity>(IQueryable<TEntity> queryable, IEnumerable<Sort> sort)
    {
        if (sort.Any())
        {
            string ordering = string.Join(",", sort.Select(s => $"{s.Field} {s.Dir}"));
            return queryable.OrderBy(ordering);
        }

        return queryable;
    }

    // Filter object is including IEnumerable<Filter>. So this method is using for get all filters into one list.
    public static IList<Filter> GetAllFilters(Filter filter)
    {
        List<Filter> filters = new();
        GetFilters(filter, filters);
        return filters;
    }

    // The filter may contain filters, and these filters may also contain filters. This function is recursive function.
    private static void GetFilters(Filter filter, IList<Filter> filters)
    {
        filters.Add(filter);
        if (filter.Filters?.Any() == true)
            foreach (Filter item in filter.Filters)
                GetFilters(item, filters);
    }

    // Prepare the predicate for use in the Where method. This method is calling from Filter method
    public static string Transform(Filter filter, IList<Filter> filters)
    {
        // Finding main filter index.
        int index = filters.IndexOf(filter);
        // Finding filter operator
        string comparison = Operators[filter.Operator];
        // Creating StringBuilder for prepare predicate of where method.
        StringBuilder predicate = new();

        // When isnull or isnotnull operators came then value will be null.
        if (!string.IsNullOrEmpty(filter.Value))
        {
            if (filter.Operator == "doesnotcontain")
                predicate.Append($"(!np({filter.Field}).{comparison}(@{index}))");
            else if (comparison == "StartsWith" ||
                     comparison == "EndsWith" ||
                     comparison == "Contains")
                predicate.Append($"(np({filter.Field}).{comparison}(@{index}))");
            else
                predicate.Append($"np({filter.Field}) {comparison} @{index}");
        }
        else if (filter.Operator == "isnull" || filter.Operator == "isnotnull")
            predicate.Append($"np({filter.Field}) {comparison}");

        if (filter.Logic is not null && filter.Filters?.Any() == true)
            return $"{predicate} {filter.Logic} ({string.Join($" {filter.Logic} ", filter.Filters.Select(f => Transform(f, filters)).ToArray())})";

        return predicate.ToString();
    }
}