using Multichain.Models;
using Newtonsoft.Json.Linq;
using System.Web.Http;

namespace FinalTestAPI.Model.Controllers
{
    public class AccountController : ApiController
    {

        private readonly IUserService user = new UserService();

        [Route("api/Account/SignUp")]
        [BasicAuthorizationAttributeApp]
        [HttpPost]
        public IHttpActionResult CreateAccount([FromBody] Input.SignUpInput input)
        {
            var result = user.SignUp(input);
            return Ok(result);
        }

        [HttpPut]
        public IHttpActionResult Confirm([FromBody]Input.ComfirmInput input)
        {
            var result = user.Confirm(input);
            return Ok(result);
        }

        [BasicAuthorizationAttributeApp]
        [HttpPost]
        public IHttpActionResult LoginAccount([FromBody]Input.LoginInput input)
        {
            var result = user.Login(input);
            return Ok(result);
        }
    }
}