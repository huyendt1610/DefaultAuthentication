using DefaultAuthentication.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DefaultAuthentication.JwtToken
{
    public class TokenRefresher : ITokenRefresher
    {
        private readonly string _key;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public TokenRefresher(string key, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _key = key;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }
        public AuthenticationResponse Refersh(RefreshCred refreshCred)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(refreshCred.JwtToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token passed!");
            }

            var username = principal.Identity.Name;

            if (!_jwtAuthenticationManager._usersRefreshTokens.ContainsKey(username)
                || refreshCred.RefreshToken != _jwtAuthenticationManager._usersRefreshTokens[username])
            {
                throw new SecurityTokenException("Invalid Token passed!");
            }
            return _jwtAuthenticationManager.Authenticate(username, principal.Claims.ToArray());
        }
    }
}
