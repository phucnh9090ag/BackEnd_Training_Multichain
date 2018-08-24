using Multichain.Models.Database;
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

        public object Login(LoginInput input)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["username"] = input.email;
                data["password"] = input.password;
                data["grant_type"] = Properties.Resources.grant_type_password;

                try
                { 
                    var response = wb.UploadValues(
                        address: WebConfigurationManager.AppSettings["tokenURL"], 
                        method: WebConfigurationManager.AppSettings["tokenMothod"], 
                        data: data
                    );

                    string responseInString = Encoding.UTF8.GetString(response);

                    var account = database.FindAccountWithEmail(
                        email: input.email
                    );

                    if (account != null)
                    {
                        account.beartoken = JObject.Parse(responseInString)["access_token"].ToString();
                        database.getDatabase().SaveChanges();
                    }
                    return responseInString;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                
            }
        }
    }
}