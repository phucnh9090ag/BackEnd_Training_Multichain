using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Web.Configuration;
using Multichain.Models.Database;

namespace Multichain.Models.Services.User
{
    public class UserService: IUserService
    {
        private readonly IDatabase database;

        public UserService()
        {
            database = new Database.Database();
        }
        public Account ValidateUser(string email, string password)
        {
            var acc = database.FindAccountWithEmail(email);
            if (acc != null)
            {
                if (acc.password == password)
                    return acc;
            }
            return null;
        }
    }
}