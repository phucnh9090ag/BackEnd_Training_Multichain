using Multichain.Models.InputControler;
using Multichain.Models.Services.Confirm;
using Multichain.Models.Services.Login;
using Multichain.Models.Services.SignUp;
using Multichain.Models.Token;
using System.Web.Http;

namespace FinalTestAPI.Model.Controllers
{
    public class AccountController : ApiController
    {

        private readonly ISignUpService signupService = new SignUpService();
        private readonly IConfirmService confirmService = new ConfirmService();
        private readonly ILoginService loginService = new LoginService();

        [Route("api/Account/SignUp")]
        [BasicAuthorizationAttributeApp]
        [HttpPost]
        public IHttpActionResult CreateAccount([FromBody] Input.SignUpInput input)
        {
            var result = signupService.SignUp(input);
            return Ok(result);
        }

        [HttpPut]
        public IHttpActionResult Confirm([FromBody]Input.ComfirmInput input)
        {
            var result = confirmService.Confirm(input);
            return Ok(result);
        }

        [BasicAuthorizationAttributeApp]
        [HttpPost]
        public IHttpActionResult LoginAccount([FromBody]Input.LoginInput input)
        {
            var result = loginService.Login(input);
            return Ok(result);
        }
    }
}