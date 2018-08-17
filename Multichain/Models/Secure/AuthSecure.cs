using Multichain.Models.Database;
using System.Web.Configuration;

namespace MultiChain.Models.Secure
{
    public class AuthSecure
    {
        private readonly IDatabase database;

        public AuthSecure()
        {
            database = new Database();
        }
        public bool AuthApp(string username, string password)
        {
            return username == WebConfigurationManager.AppSettings["usenameApp"] && password == WebConfigurationManager.AppSettings["passwordApp"];
        }

        public bool AuthAccount(string username, string password)
        {
            var acc = database.FindAccountWithEmail(username);
            if (acc != null)
            {
                if (acc.password == password)
                    return true;
            }
            return false;
        }
    }
}