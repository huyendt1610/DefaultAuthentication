using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DefaultAuthentication.JwtToken
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IDictionary<string, string> _users = new Dictionary<string, string>()
        {
            { "user1", "password1"},  { "user2", "password2"}
        };

        //shoud store in some sort of db (ex redis)
        //public IDictionary<string, string> _usersRefreshTokens = new Dictionary<string, string>();
        public IDictionary<string, string> _usersRefreshTokens => new Dictionary<string, string>();

        private readonly string _key;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public JwtAuthenticationManager(string key, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _key = key;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public AuthenticationResponse Authenticate(string username, Claim[] claims)
        {
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
                );
            var newJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            if (_usersRefreshTokens.ContainsKey(username))
            {
                _usersRefreshTokens[username] = refreshToken;
            }
            else
            {
                _usersRefreshTokens.Add(username, refreshToken);
            }

            return new AuthenticationResponse { JwtToken = newJwtToken, RefreshToken = refreshToken };

        }

        public AuthenticationResponse Authenticate(string username, string password)
        {
            //change to get from db 
            if (!_users.Any(e => e.Key == username && e.Value == password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                 }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            if (_usersRefreshTokens.ContainsKey(username))
            {
                _usersRefreshTokens[username] = refreshToken;
            }
            else
            {
                _usersRefreshTokens.Add(username, refreshToken);
            }

            return new AuthenticationResponse() { JwtToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
        }
    }
}
