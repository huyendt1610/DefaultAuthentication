namespace DefaultAuthentication.CustomHandler
{
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        private readonly IDictionary<string, string> _users = new Dictionary<string, string>()
        {
            { "user1", "password1"},  { "user2", "password2"}
        };
        //should be stored in some distributed cache with an expiry
        private readonly IDictionary<string, string> _tokens = new Dictionary<string, string>();

        public IDictionary<string, string> Tokens => _tokens; 
        public string Authenticate(string username, string password)
        {
            if (!_users.Any(e => e.Key == username && e.Value == password))
            {
                return null;
            }

            var token = Guid.NewGuid().ToString();  
            _tokens.Add( token, username);

            return token;
        }
    }
}
