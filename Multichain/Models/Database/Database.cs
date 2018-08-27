using System;
using System.Linq;

namespace Multichain.Models.Database
{
    public class Database:IDatabase
    {
        private DBEntity _DB;

        public Database()
        {
            _DB = new DBEntity();
        }

        public Account FindAccountWithEmail(string email)
        {
            var account = _DB.Accounts.SingleOrDefault(acc => acc.email == email);
            return account;
        }

        public Account FindAccountWithOTP(string OTP)
        {
            var account = _DB.Accounts.SingleOrDefault(acc => acc.OTP == OTP);
            return account;
        }

        public DBEntity getDatabase()
        {
            return _DB;
        }

        public bool SaveAccount(Account account)
        {
            try
            { 
                _DB.Accounts.Add(account);
                _DB.SaveChanges();
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
                _DB.Addresses.Add(address);
                _DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}