﻿namespace Core.Application.DynamicQuery;

public class Filter
{
    public Filter()
    {
    }

    public Filter(string field, string @operator, string? value, string? logic, IEnumerable<Filter>? filters) : this()
    {
        Field = field;
        Operator = @operator;
        Value = value;
        Logic = logic;
        Filters = filters;
    }

    public string Field { get; set; } = null!;
    public string Operator { get; set; } = null!;
    public string? Value { get; set; }
    public string? Logic { get; set; }
    public IEnumerable<Filter>? Filters { get; set; }
}