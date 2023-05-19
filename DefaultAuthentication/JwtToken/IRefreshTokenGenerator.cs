namespace DefaultAuthentication.JwtToken
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken();
    }
}