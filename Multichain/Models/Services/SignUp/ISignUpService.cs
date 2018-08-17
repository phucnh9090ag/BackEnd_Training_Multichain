using Multichain.Models.Database;
using Multichain.Models.InputControler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models.Services.SignUp
{
    interface ISignUpService
    {
        object SignUp(Input.SignUpInput input);
        string CreateOTP();
        bool SaveDatabase(Account account);
        bool isAccountValid(Account account);
    }
}
