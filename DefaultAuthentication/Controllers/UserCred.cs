namespace DefaultAuthentication.Controllers
{
    public class UserCred
    {
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class RefreshCred {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}