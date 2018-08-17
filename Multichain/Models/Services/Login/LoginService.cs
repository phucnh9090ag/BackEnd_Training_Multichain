using Multichain.Models.Database;
using Multichain.Models.InputControler;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.Configuration;

namespace Multichain.Models.Services.Login
{
    public class LoginService:ILoginService
    {
        private readonly IDatabase database;

        public LoginService()
        {
            database = new Database.Database();
        }
        public object Login(Input.LoginInput input)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["username"] = input.email;
                data["password"] = input.password;
                data["grant_type"] = Properties.Resources.grant_type_password;

                try
                { 
                    var response = wb.UploadValues(WebConfigurationManager.AppSettings["tokenURL"], WebConfigurationManager.AppSettings["tokenMothod"], data);
                    string responseInString = Encoding.UTF8.GetString(response);

                    var account = database.FindAccountWithEmail(input.email);
                    if (account != null)
                    {
                        account.beartoken = JObject.Parse(responseInString)["access_token"].ToString();
                        database.getDatabase().SaveChanges();
                    }
                    return
                        JObject.Parse(responseInString);
                }
                catch (Exception ex)
                {
                    return JObject.FromObject(new { message = Properties.Resources.LoginFail, Error = ex.Message });
                }
                
            }
        }
    }
}