using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models.Services.Mail
{
    interface IMailServices
    {
        bool SendEmail(string context, string To);
    }
}
