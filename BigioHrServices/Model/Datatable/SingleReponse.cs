namespace BigioHrServices.Model.Datatable
{
    public class SingleReponse<T>
    {
        public SingleReponse(List<T> items)
        {
            Data =items;
        }
        public object Data { get; set; }
    }
}
