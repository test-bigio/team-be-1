namespace BigioHrServices.Model
{
    public class BaseResponse
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static BaseResponse FromException(Exception ex)
        {
            return new BaseResponse
            {
                isSuccess = false,
                Message = ex.Message
            };
        }
    }
}
