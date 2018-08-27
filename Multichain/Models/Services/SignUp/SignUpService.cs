using Multichain.Models.Database;
using Multichain.Models.Services.Mail;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Configuration;

namespace Multichain.Models.Services.SignUp
{
    public class SignUpService:ISignUpService
    {

        private Random _rand;
        private int _MaxOTP = Convert.ToInt32(WebConfigurationManager.AppSettings["MaxOTP"]);
        private int _MinOTP = Convert.ToInt32(WebConfigurationManager.AppSettings["MinOTP"]);

        private readonly IDatabase _database;
        private readonly IMailServices _mailService;
        private Account _account;

        public SignUpService()
        {
            _database = new Models.Database.Database();
            _mailService = new MailService();
            _account = new Account();
            _rand = new Random();
        }

        public object SignUp(SignUpInput input)
        {
            _account.email = input.Email;
            _account.password = input.Password;

            if (isAccountValid(_account))
                return JObject.Parse(Properties.Resources.AccountValidError);

            _account.OTP = CreateOTP();

            if (_mailService.SendEmail(_account.OTP, _account.email))
            {
                SaveDatabase(_account);
                return JObject.FromObject(new { email = _account.email, message = Properties.Resources.SentEmailMessage });
            }
            else
                return JObject.FromObject(new { error = Properties.Resources.SendMailError });

        }

        public string CreateOTP()
        {
            var _OTP = _rand.Next(_MinOTP, _MaxOTP).ToString();
            while (_database.getDatabase().Accounts.Where(acc => acc.OTP == _OTP).Count() != 0)
                _OTP = _rand.Next(_MinOTP, _MaxOTP).ToString();

            return _OTP;
        }

        public bool isAccountValid(Account account)
        {
            var ac = _database.getDatabase().Accounts.Find(account.email);
            if (ac != null)
                return true;
            return false;
        }

        public bool SaveDatabase(Account account)
        {
            _database.SaveAccount(account);
            return true;
        }
    }
}