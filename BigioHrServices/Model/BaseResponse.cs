namespace BigioHrServices.Model
{
    public class BaseResponse
    {
        public bool isSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
