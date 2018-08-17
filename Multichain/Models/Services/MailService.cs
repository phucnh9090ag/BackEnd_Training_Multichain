using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace Multichain.Models
{
    public class MailService
    {
        private SmtpClient smtpClient;
        public MailService()
        {

            var hostMail = WebConfigurationManager.AppSettings["HostMail"];
            var portMail = Convert.ToInt32(WebConfigurationManager.AppSettings["PortMail"]);
            smtpClient = new SmtpClient(hostMail, portMail);

            smtpClient.UseDefaultCredentials = true;

            var fromMail = WebConfigurationManager.AppSettings["MailFrom"];
            var passwordFromMail = WebConfigurationManager.AppSettings["PasswordMailFrom"];
            smtpClient.Credentials = new NetworkCredential(fromMail, passwordFromMail);

            smtpClient.EnableSsl = true;
        }
        internal bool SendEmail(string OTP, string toEmail)
        {
            string mailBodyhtml ="<p>OTP code: " + OTP + "</p>";
            string fromEmail = WebConfigurationManager.AppSettings["MailFrom"];
            var Subject = WebConfigurationManager.AppSettings["SubjectMail"];
            var msg = new MailMessage(fromEmail, toEmail, Subject, mailBodyhtml);

            msg.IsBodyHtml = true;
 
            try
            {
                smtpClient.Send(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}