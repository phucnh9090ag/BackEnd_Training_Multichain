using Multichain.Models;
using System.Web.Configuration;

namespace FinalTestAPI.Model.Models
{
    public class AuthSecure
    {
        DBEntity database = new DBEntity();
        public bool AuthApp(string username, string password)
        {
            return username == WebConfigurationManager.AppSettings["usenameApp"] && password == WebConfigurationManager.AppSettings["passwordApp"];
        }

        public bool AuthAccount(string username, string password)
        {
            var acc = database.Accounts.Find(username);
            if (acc != null)
            {
                if (acc.password == password)
                    return true;
            }
            return false;
        }
    }
}