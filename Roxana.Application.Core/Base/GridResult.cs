namespace Roxana.Application.Core.Base;

public class GridFilterWithParams<T> : GridFilter
{
    public T Params { get; set; }
}

public class GridFilter
{
    public GridFilter()
    {
        PageSize = 20;
    }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public string FilterBy { get; set; }
    public string SortBy { get; set; }
    public GridSortOrder SortOrder { get; set; }
}

public class GridResult<T>
{
    public T[] Items { get; set; }
    
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }

    public double TotalPages => Math.Ceiling(TotalItems / (double)PageSize);
}

public enum GridSortOrder
{
    Ascending = 1,
    Descending = 2
}