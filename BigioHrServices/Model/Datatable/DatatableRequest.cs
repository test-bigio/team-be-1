namespace BigioHrServices.Model.Datatable
{
    public class DatatableRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool Desc { get; set; } = true;
        public string Search { get; set; } = string.Empty;
    }
}
