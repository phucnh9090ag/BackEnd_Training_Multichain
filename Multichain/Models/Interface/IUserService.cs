using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models
{
    interface IUserService
    {
        object SignUp(Input.SignUpInput input);
        object Confirm(Input.ComfirmInput input);
        object Login(Input.LoginInput input);
        Account ValidateUser(string email, string password);
    }
}
