using System;
using System.Linq;

namespace Multichain.Models.Database
{
    public class Database:IDatabase
    {
        private DBEntity DB;

        public Database()
        {
            DB = new DBEntity();
        }

        public Account FindAccountWithEmail(string email)
        {
            var account = DB.Accounts.SingleOrDefault(acc => acc.email == email);
            return account;
        }

        public Account FindAccountWithOTP(string OTP)
        {
            var account = DB.Accounts.SingleOrDefault(acc => acc.OTP == OTP);
            return account;
        }

        public DBEntity getDatabase()
        {
            return DB;
        }

        public bool SaveAccount(Account account)
        {
            try
            { 
                DB.Accounts.Add(account);
                DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveAddress(Address address)
        {
            try
            {
                DB.Addresses.Add(address);
                DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}