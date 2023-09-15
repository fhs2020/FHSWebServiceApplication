namespace FHSWebServiceApplication.Model
{
    public class LoginResult
    {
        public string Message { get; set; }

        public string Email { get; set; }

        public string JwtBearer { get; set; }

        public bool Success { get; set; }
    }
}
