using Multichain.Models.Database;
using Multichain.Models.Services.Multichain;
using System;

namespace Multichain.Models.Services.Confirm
{
    public class ConfirmService:IConfirmService
    {

        private IDatabase _database;
        private Address _address;
        private readonly IMultichainServices _multichainServices;

        public ConfirmService()
        {
            _database = new Database.Database();
            _multichainServices = new MultichainServices();
        }

        public object Confirm(ComfirmInput input)
        {
            var account = _database.FindAccountWithOTP(input.OTP);
            if (account != null)
            {
                try
                {
                    _address = _multichainServices.CreateAddress();
                    _address.email = account.email;

                    _database.SaveAddress(_address);

                    account.OTP = null;
                    _database.getDatabase().SaveChanges();

                    return new { email = account.email, address = _address.addr };
                }
                catch (Exception ex)
                {
                    return new { result = ex.Message };
                }
            }
            else
                return new { result = false };
        }
    }
}