using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;

namespace DefaultAuthentication.CustomHandler
{
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        private readonly ICustomAuthenticationManager _customAuthenticationManager;

        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ICustomAuthenticationManager customAuthenticationManager
            ) : base(options, logger, encoder, clock)
        {
            _customAuthenticationManager = customAuthenticationManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            if (!authorizationHeader.StartsWith("bearer", StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var token = authorizationHeader.Substring("bearer".Length).Trim();
            try
            {
                var rs = ValidateToken(token);
                return rs;
            }
            catch (Exception ex)
            {
                //Log ex
                return AuthenticateResult.Fail("Unauthorized");
            }

        }

        private AuthenticateResult ValidateToken(string token)
        {
            if (!_customAuthenticationManager.Tokens.ContainsKey(token))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
            var name = _customAuthenticationManager.Tokens[token];

            //var validatedToken = _customAuthenticationManager.Tokens.FirstOrDefault(e => e.Key == token); 

            //if(validatedToken.Key == null) { return AuthenticateResult.Fail("Unauthorized"); }

            // var claims = new List<Claim> { new Claim(ClaimTypes.Name, validatedToken.Value) };
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, name) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
