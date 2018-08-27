using Multichain.Models.Services.Confirm;
using Multichain.Models.Services.Login;
using Multichain.Models.Services.SignUp;
using Multichain.Models.Token;
using System.Web.Http;

namespace FinalTestAPI.Model.Controllers
{
    public class AccountController : ApiController
    {
        private readonly ISignUpService _signupService;
        private readonly IConfirmService _confirmService;
        private readonly ILoginService _loginService;

        public AccountController()
        {
            _signupService = new SignUpService(); 
            _confirmService = new ConfirmService();
            _loginService = new LoginService();
        }

        [Route("api/Account/SignUp")]
        [BasicAuthorizationAttributeApp]
        [HttpPost]
        public IHttpActionResult CreateAccount([FromBody]SignUpInput input)
        {
            var result = _signupService.SignUp(input);
            return Ok(result);
        }

        [HttpPut]
        public IHttpActionResult Confirm([FromBody]ComfirmInput input)
        {
            var result = _confirmService.Confirm(input);
            return Ok(result);
        }

        [BasicAuthorizationAttributeApp]
        [HttpPost]
        public IHttpActionResult LoginAccount([FromBody]LoginInput input)
        {
            var result = _loginService.Login(input);
            return Ok(result);
        }
    }
}