namespace Core.Application.DynamicQuery;

public class Dynamic
{
    public Dynamic()
    {
    }

    public Dynamic(string? select, Filter? filter, IEnumerable<Sort>? sort)
    {
        Select = select;
        Filter = filter;
        Sort = sort;
    }

    public string? Select { get; set; }
    public Filter? Filter { get; set; }
    public IEnumerable<Sort>? Sort { get; set; }
}