namespace DefaultAuthentication.CustomHandler
{
    public interface ICustomAuthenticationManager
    {
        string Authenticate(string username, string password);
        IDictionary<string, string> Tokens { get; }
    }
}