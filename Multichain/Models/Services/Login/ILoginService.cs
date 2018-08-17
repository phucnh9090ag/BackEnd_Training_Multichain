using Multichain.Models.InputControler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models.Services.Login
{
    interface ILoginService
    {
        object Login(Input.LoginInput input);
    }
}
