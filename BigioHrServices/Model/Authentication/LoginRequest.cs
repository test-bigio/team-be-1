namespace BigioHrServices.Model.Authentication
{
    public class LoginRequest
    {
        public string NIK { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
