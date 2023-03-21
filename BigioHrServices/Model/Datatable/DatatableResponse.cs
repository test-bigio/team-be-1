namespace BigioHrServices.Model.Datatable
{
    public class DatatableResponse
    {
        public DatatableResponse()
        {
            Data = new List<object>();
        }
        public bool NextPage { get; set; }
        public bool PrevPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public object Data { get; set; }
    }
}
