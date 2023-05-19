using System.Security.Claims;

namespace DefaultAuthentication.JwtToken
{
    public interface IJwtAuthenticationManager
    {
        AuthenticationResponse Authenticate(string username, string password);
        IDictionary<string, string> _usersRefreshTokens { get; }
        AuthenticationResponse Authenticate(string username, Claim[] claims);
    }
}