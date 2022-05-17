namespace Hospital.Application.Results.Authenticate
{
    public class LoginResult
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
