using Multichain.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multichain.Models.Database
{
    interface IDatabase
    {
        DBEntity getDatabase();
        bool SaveAccount(Account account);
        bool SaveAddress(Address address);
        Account FindAccountWithOTP(string OTP);
        Account FindAccountWithEmail(string email);
    }
}
