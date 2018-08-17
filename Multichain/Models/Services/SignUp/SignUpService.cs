using Multichain.Models.Database;
using Multichain.Models.InputControler;
using Multichain.Models.Services.Mail;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Configuration;

namespace Multichain.Models.Services.SignUp
{
    public class SignUpService:ISignUpService
    {

        private Random rand;
        private int MaxOTP = Convert.ToInt32(WebConfigurationManager.AppSettings["MaxOTP"]);
        private int MinOTP = Convert.ToInt32(WebConfigurationManager.AppSettings["MinOTP"]);

        private readonly IDatabase database;
        private readonly IMailServices mailService;
        private Account account;

        public SignUpService()
        {
            database = new Models.Database.Database();
            mailService = new MailService();
            account = new Account();
            rand = new Random();
        }

        public object SignUp(Input.SignUpInput input)
        {
            account.email = input.email;
            account.password = input.password;

            if (isAccountValid(account))
                return JObject.Parse(Properties.Resources.AccountValidError);

            account.OTP = CreateOTP();

            if (mailService.SendEmail(account.OTP, account.email))
            {
                SaveDatabase(account);
                return JObject.FromObject(new { email = account.email, message = Properties.Resources.SentEmailMessage });
            }
            else
                return JObject.FromObject(new { error = Properties.Resources.SendMailError });

        }

        public string CreateOTP()
        {
            var _OTP = rand.Next(MinOTP, MaxOTP).ToString();
            while (database.getDatabase().Accounts.Where(acc => acc.OTP == _OTP).Count() != 0)
                _OTP = rand.Next(MinOTP, MaxOTP).ToString();

            return _OTP;
        }

        public bool isAccountValid(Account account)
        {
            var ac = database.getDatabase().Accounts.Find(account.email);
            if (ac != null)
                return true;
            return false;
        }

        public bool SaveDatabase(Account account)
        {
            database.SaveAccount(account);
            return true;
        }
    }
}