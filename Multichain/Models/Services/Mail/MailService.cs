using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace Multichain.Models.Services.Mail
{
    public class MailService: IMailServices
    {
        private SmtpClient _smtpClient;
        public MailService()
        {

            var hostMail = WebConfigurationManager.AppSettings["HostMail"];
            var portMail = Convert.ToInt32(WebConfigurationManager.AppSettings["PortMail"]);
            _smtpClient = new SmtpClient(hostMail, portMail);

            _smtpClient.UseDefaultCredentials = true;

            var fromMail = WebConfigurationManager.AppSettings["MailFrom"];
            var passwordFromMail = WebConfigurationManager.AppSettings["PasswordMailFrom"];
            _smtpClient.Credentials = new NetworkCredential(fromMail, passwordFromMail);

            _smtpClient.EnableSsl = true;
        }
        public bool SendEmail(string context, string To)
        {
            string mailBodyhtml =Properties.Resources.HeaderMail + context;
            string fromEmail = WebConfigurationManager.AppSettings["MailFrom"];
            var Subject = WebConfigurationManager.AppSettings["SubjectMail"];
            var msg = new MailMessage(fromEmail, To, Subject, mailBodyhtml);

            msg.IsBodyHtml = true;
 
            try
            {
                _smtpClient.Send(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}