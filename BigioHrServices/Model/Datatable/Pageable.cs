namespace BigioHrServices.Model.Datatable;

public class Pageable<T>
{
    public List<T> Content { get; }
    public int TotalContent { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage => PageNumber > 0;
    public bool HasNextPage => PageNumber < TotalPages;

    public Pageable(List<T> items, int pageNumber, int pageSize)
    {
        TotalContent = items.Count;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalContent / (double)PageSize) - 1;

        Content = items.Skip(PageNumber * PageSize)
            .Take(PageSize)
            .ToList();
    }
}