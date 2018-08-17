using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace Multichain.Models
{
    public class UserService: IUserService
    {
        private int MaxOTP = Convert.ToInt32(WebConfigurationManager.AppSettings["MaxOTP"]);
        private int MinOTP = Convert.ToInt32(WebConfigurationManager.AppSettings["MinOTP"]);

        Random rand = new Random();
        DBEntity database = new DBEntity();
        MailService mailService = new MailService();

        public UserService()
        {
        }

        public object SignUp(Input.SignUpInput input)
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            if (username != WebConfigurationManager.AppSettings["usenameApp"])
            {
                return 
                    JObject.Parse(Properties.Resources.UnAuthError);
            }

            var _email = input.email;
            var _pass = input.password;
            var _OTP = rand.Next(MinOTP, MaxOTP).ToString();
            while (database.Accounts.Where(acc => acc.OTP == _OTP).Count() != 0)
                _OTP = rand.Next(MinOTP, MaxOTP).ToString();

            var ac = database.Accounts.Find(_email);
            if (ac != null)
                return
                    JObject.Parse(Properties.Resources.AccountValidError);



            if (mailService.SendEmail(_OTP, _email))
            {
                database.Accounts.Add(new Account { password = _pass, email = _email, OTP = _OTP });
                database.SaveChanges();
                return
                    JObject.FromObject(new { email = _email, message = Properties.Resources.SentEmailMessage });
            }
            else
                return
                    JObject.FromObject(new { error = Properties.Resources.SendMailError });
            
        }

        public object Confirm(Input.ComfirmInput input)
        {
            var _OTP = input.OTP;
            var _acc = database.Accounts.SingleOrDefault(acc => acc.OTP == _OTP);
            if (_acc != null)
            {
                try
                {
                    var server = new MultichainServices();
                    var infoAddress = server.CreateAddress();
                    var _addr = infoAddress.addr;
                    var _privateKey = infoAddress.privateKey;

                    database.Addresses.Add(new Address { addr = _addr, privateKey = _privateKey, email = _acc.email });

                    _acc.OTP = null;
                    database.SaveChanges();
                    return 
                        new { email = _acc.email, address = infoAddress.addr };
                }
                catch (Exception ex)
                {
                    return 
                        new { result = ex.Message };
                }
            }

            return 
                new { result = false };
        }

        public object Login(Input.LoginInput input)
        {
            using (var wb = new WebClient())
            {
                var _email = input.email;
                var _pass = input.password;

                var data = new NameValueCollection();
                data["username"] = _email;
                data["password"] = _pass;
                data["grant_type"] = Properties.Resources.grant_type_password;

                var response = wb.UploadValues(WebConfigurationManager.AppSettings["tokenURL"], WebConfigurationManager.AppSettings["tokenMothod"], data);
                string responseInString = Encoding.UTF8.GetString(response);

                var _acc = database.Accounts.SingleOrDefault(acc => acc.email == _email);
                if (_acc != null)
                    _acc.beartoken = JObject.Parse(responseInString)["access_token"].ToString();
                database.SaveChanges();
                return 
                    JObject.Parse(responseInString);
            }
        }
        public Account ValidateUser(string email, string password)
        {
            var acc = database.Accounts.Find(email);
            if (acc != null)
            {
                if (acc.password == password)
                    return 
                        acc;
            }
            return 
                null;
        }
    }
}