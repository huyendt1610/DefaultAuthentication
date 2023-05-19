using DefaultAuthentication.Controllers;

namespace DefaultAuthentication.JwtToken
{
    public interface ITokenRefresher
    {
        AuthenticationResponse Refersh(RefreshCred refreshCred);
    }
}