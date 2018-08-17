using Multichain.Models.Database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models.Services.User
{
    interface IUserService
    {
        Account ValidateUser(string email, string password);
    }
}
