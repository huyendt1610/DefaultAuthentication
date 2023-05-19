using DefaultAuthentication.CustomHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DefaultAuthentication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NameCustomAuthController : ControllerBase
    {
        private readonly ICustomAuthenticationManager _customAuthenticationManager;

        public NameCustomAuthController(ICustomAuthenticationManager customAuthenticationManager)
        {
            _customAuthenticationManager = customAuthenticationManager;
        }

        // GET: api/<NameController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NameController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = _customAuthenticationManager.Authenticate(userCred.Username, userCred.Password); 
            if (token == null)
            {
                return Unauthorized(); 
            }
            return Ok(token);
        }

        
    }
}
