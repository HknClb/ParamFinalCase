namespace Core.Application.DynamicQuery;

public class Sort
{
    public string Field { get; set; } = null!;
    public string Dir { get; set; } = null!;

    public Sort()
    {
    }

    public Sort(string field, string dir) : this()
    {
        Field = field;
        Dir = dir;
    }
}